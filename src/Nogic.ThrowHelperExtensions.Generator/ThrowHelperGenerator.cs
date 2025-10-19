using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Nogic.ThrowHelperExtensions.Generator;

[Generator(LanguageNames.CSharp)]
public class ThrowHelperGenerator : IIncrementalGenerator
{
    private const string EmbeddedAttribute = "Microsoft.CodeAnalysis.EmbeddedAttribute";
    private static readonly Regex EmbeddedResourceNameToFullyQualifiedTypeNameRegex = new(@"^Nogic\.ThrowHelperExtensions\.Generator\.EmbeddedResources\.(\w+(?:\.\w+)+)\.cs$", RegexOptions.Compiled);

    /// <summary>
    /// Dictionary of embedded resource names mapped to their fully qualified type names.
    /// </summary>
    public static readonly ImmutableDictionary<string, string> EmbeddedResources = ImmutableDictionary.CreateRange(
        typeof(ThrowHelperGenerator).Assembly.GetManifestResourceNames()
            .Select(n => new KeyValuePair<string, string>(EmbeddedResourceNameToFullyQualifiedTypeNameRegex.Match(n).Groups[1].Value, n))
    );

    /// <summary>
    /// Cache for source texts of generated types.
    /// </summary>
    private readonly ConcurrentDictionary<string, SourceText> manifestSources = new();

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Generate EmbeddedAttribute first
        context.RegisterPostInitializationOutput(EmitEmbeddedAttribute);

        // Generate ExceptionPolyfills and necessary attributes
        var availableTypes = context.CompilationProvider.SelectMany(GetNeedGenerateTypes);
        context.RegisterSourceOutput(availableTypes, this.EmitGeneratedType);
    }

    /// <summary>
    /// Generates EmbeddedAttribute source code.
    /// </summary>
    /// <param name="context">Context for post-initialization.</param>
    private static void EmitEmbeddedAttribute(IncrementalGeneratorPostInitializationContext context)
    {
        if (context.CancellationToken.IsCancellationRequested)
            return;

        if (!EmbeddedResources.TryGetValue(EmbeddedAttribute, out string? resource))
            return;
        using var stream = typeof(ThrowHelperGenerator).Assembly.GetManifestResourceStream(resource)!;
        using var reader = new StreamReader(stream);
        string source = reader.ReadToEnd();
        context.AddSource($"{EmbeddedAttribute}.g.cs", source);
    }

    /// <summary>
    /// Emits a generated source file for the specified type into the compilation context.
    /// </summary>
    /// <param name="context">The source production context.</param>
    /// <param name="typeName">The name of the type for which the generated source file should be emitted.</param>
    private void EmitGeneratedType(SourceProductionContext context, string typeName)
    {
        if (context.CancellationToken.IsCancellationRequested)
            return;

        if (!this.manifestSources.TryGetValue(typeName, out var sourceText))
        {
            string resource = EmbeddedResources[typeName];
            using var stream = typeof(ThrowHelperGenerator).Assembly.GetManifestResourceStream(resource)!;
            sourceText = SourceText.From(stream, Encoding.UTF8, canBeEmbedded: true);

            _ = this.manifestSources.TryAdd(typeName, sourceText);
        }
        context.AddSource($"{typeName}.g.cs", sourceText);
    }

    private static ImmutableArray<string> GetNeedGenerateTypes(Compilation compilation, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        if (((CSharpCompilation)compilation).LanguageVersion < (LanguageVersion)1400) // ExceptionPolyfills uses C# 14.0 features
            return ImmutableArray<string>.Empty;

        var builder = ImmutableArray.CreateBuilder<string>();
        foreach (var kvp in EmbeddedResources)
        {
            string fullTypeName = kvp.Key;
            if (fullTypeName != EmbeddedAttribute && !IsTypeAlreadyExists(compilation, fullTypeName, token))
                builder.Add(fullTypeName);
        }
        return builder.ToImmutable();

        static bool IsTypeAlreadyExists(Compilation compilation, string fullTypeName, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            if (compilation.GetTypeByMetadataName(fullTypeName) is INamedTypeSymbol typeSymbol)
                return compilation.IsSymbolAccessibleWithin(typeSymbol, compilation.Assembly);

            foreach (var item in compilation.GetTypesByMetadataName(fullTypeName))
            {
                if (compilation.IsSymbolAccessibleWithin(item, compilation.Assembly))
                    return true;
            }

            return false;
        }
    }
}
