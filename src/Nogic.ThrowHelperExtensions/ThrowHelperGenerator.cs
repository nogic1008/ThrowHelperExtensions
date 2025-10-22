using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Nogic.ThrowHelperExtensions;

[Generator(LanguageNames.CSharp)]
public class ThrowHelperGenerator : IIncrementalGenerator
{
    private const string EmbeddedAttribute = "Microsoft.CodeAnalysis.EmbeddedAttribute";
    private static readonly Regex EmbeddedResourceNameToFullyQualifiedTypeNameRegex = new(@"^Nogic\.ThrowHelperExtensions\.EmbeddedResources\.(\w+(?:\.\w+)+)\.cs$", RegexOptions.Compiled);

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

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Generate EmbeddedAttribute first (always safe to generate)
        context.RegisterPostInitializationOutput(EmitEmbeddedAttribute);

        // Generate ExceptionPolyfills and necessary attributes with options
        var buildOptions = context.AnalyzerConfigOptionsProvider.Select(static (options, token) =>
        {
            token.ThrowIfCancellationRequested();
            var globalOptions = options.GlobalOptions;
            bool generateAttributes = true;

            // Try to read the build property
            if (globalOptions.TryGetValue("build_property.ThrowHelperExtensionsGenerateAttributes", out string? generateValue))
            {
                generateAttributes = !string.Equals(generateValue, "false", StringComparison.OrdinalIgnoreCase);
            }

            return generateAttributes;
        });
        var compilationWithConfig = context.CompilationProvider.Combine(buildOptions);
        var availableTypes = compilationWithConfig.SelectMany(static (x, token) => GetNeedGenerateTypes(x.Left, x.Right, token));
        context.RegisterSourceOutput(availableTypes, this.EmitGeneratedType);
    }

    private static ImmutableArray<string> GetNeedGenerateTypes(Compilation compilation, bool generateAttributes, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        if (((CSharpCompilation)compilation).LanguageVersion < (LanguageVersion)1400) // ExceptionPolyfills uses C# 14.0 features
            return ImmutableArray<string>.Empty;

        var builder = ImmutableArray.CreateBuilder<string>();
        foreach (var kvp in EmbeddedResources)
        {
            string fullTypeName = kvp.Key;
            // Skip EmbeddedAttribute as it's handled separately in post-initialization
            if (fullTypeName == EmbeddedAttribute)
                continue;

            // Skip attribute generation if disabled via build property
            if (IsAttributeType(fullTypeName) && !generateAttributes)
                continue;

            // For other types, check if they already exist to avoid conflicts with built-in types
            if (!IsTypeAlreadyExists(compilation, fullTypeName, token))
            {
                if (!fullTypeName.EndsWith("Unsafe", StringComparison.Ordinal) || ((CSharpCompilation)compilation).Options.AllowUnsafe)
                    builder.Add(fullTypeName);
            }
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

        static bool IsAttributeType(string fullTypeName) => fullTypeName.EndsWith("Attribute", StringComparison.Ordinal);
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

        using var stream = typeof(ThrowHelperGenerator).Assembly.GetManifestResourceStream(resource);
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
            using var stream = typeof(ThrowHelperGenerator).Assembly.GetManifestResourceStream(resource);
            sourceText = SourceText.From(stream, Encoding.UTF8, canBeEmbedded: true);

            _ = this.manifestSources.TryAdd(typeName, sourceText);
        }
        context.AddSource($"{typeName}.g.cs", sourceText);
    }
}
