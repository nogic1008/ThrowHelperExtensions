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
        var availableTypes = compilationWithConfig.SelectMany(GetNeedGenerateTypes);
        context.RegisterSourceOutput(availableTypes, this.EmitGeneratedType);
    }

    /// <summary>
    /// Determines which types need to be generated based on the compilation context and configuration.
    /// </summary>
    /// <param name="config">A tuple containing the compilation and whether to generate attributes.</param>
    /// <param name="token">Cancellation token to monitor for cancellation requests.</param>
    /// <returns>An immutable array of fully qualified type names that need to be generated.</returns>
    private static ImmutableArray<string> GetNeedGenerateTypes((Compilation compilation, bool generateAttributes) config, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        if (((CSharpCompilation)config.compilation).LanguageVersion < (LanguageVersion)1400) // ExceptionPolyfills uses C# 14.0 features
            return ImmutableArray<string>.Empty;

        var builder = ImmutableArray.CreateBuilder<string>();
        foreach (var kvp in EmbeddedResources)
        {
            string fullTypeName = kvp.Key;
            if (ShouldGenerateType(fullTypeName, config.compilation, config.generateAttributes, token))
                builder.Add(fullTypeName);
        }
        return builder.ToImmutable();
    }

    /// <summary>
    /// Determines whether a type should be generated based on the configuration and compilation context.
    /// </summary>
    /// <param name="fullTypeName">The fully qualified name of the type to check.</param>
    /// <param name="compilation">The current compilation context.</param>
    /// <param name="generateAttributes">Whether attribute generation is enabled.</param>
    /// <param name="token">Cancellation token to monitor for cancellation requests.</param>
    /// <returns>True if the type should be generated; otherwise, false.</returns>
    private static bool ShouldGenerateType(string fullTypeName, Compilation compilation, bool generateAttributes, CancellationToken token)
    {
        // Skip EmbeddedAttribute as it's handled separately in post-initialization
        if (fullTypeName == EmbeddedAttribute)
            return false;

        // Skip attribute generation if disabled via build property
        if (fullTypeName.EndsWith("Attribute", StringComparison.Ordinal) && !generateAttributes)
            return false;

        // Skip if type already exists to avoid conflicts
        if (IsTypeAlreadyExists(compilation, fullTypeName, token))
            return false;

        // Skip unsafe types if unsafe code is not allowed
        return !fullTypeName.EndsWith("Unsafe", StringComparison.Ordinal) || ((CSharpCompilation)compilation).Options.AllowUnsafe;
    }

    /// <summary>
    /// Determines if a type already exists in the compilation to avoid conflicts.
    /// </summary>
    /// <param name="compilation">The current compilation context.</param>
    /// <param name="fullTypeName">The fully qualified name of the type to check.</param>
    /// <param name="token">Cancellation token to monitor for cancellation requests.</param>
    /// <remarks>
    /// This method cannot check other source generator's outputs because they are processed on other steps.
    /// </remarks>
    private static bool IsTypeAlreadyExists(Compilation compilation, string fullTypeName, CancellationToken token)
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
