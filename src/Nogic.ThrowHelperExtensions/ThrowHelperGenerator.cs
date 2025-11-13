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
    private const LanguageVersion MinimumRequiredLanguageVersion = (LanguageVersion)1400; // C# 14.0
    private static readonly Regex EmbeddedResourceNameToFullyQualifiedTypeNameRegex = new(@"^Nogic\.ThrowHelperExtensions\.EmbeddedResources\.(\w+(?:\.\w+)+)\.cs$", RegexOptions.Compiled);

    /// <summary>
    /// Diagnostic descriptor for C# language version warning.
    /// </summary>
    private static readonly DiagnosticDescriptor CSharpVersionWarning = new(
        id: "THEX0001",
        title: "C# language version must be 14 or higher",
        messageFormat: "ThrowHelperExtensions requires C# 14 or higher. Current version is {0}. Please set <LangVersion>14</LangVersion> in your project file.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        helpLinkUri: "https://github.com/nogic1008/ThrowHelperExtensions/blob/v1.0.0/README.md#usage"
    );

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

        // Check C# language version and report warning if needed
        var languageVersionProvider = context.CompilationProvider.Select(static (compilation, token) =>
        {
            token.ThrowIfCancellationRequested();
            return ((CSharpCompilation)compilation).LanguageVersion;
        });
        context.RegisterSourceOutput(languageVersionProvider, static (context, languageVersion) =>
        {
            if (languageVersion < MinimumRequiredLanguageVersion)
            {
                var diagnostic = Diagnostic.Create(CSharpVersionWarning, Location.None, languageVersion.ToDisplayString());
                context.ReportDiagnostic(diagnostic);
            }
        });

        // Generate necessary attributes
        var buildOptions = context.AnalyzerConfigOptionsProvider.Select(static (options, token) =>
        {
            token.ThrowIfCancellationRequested();
            var globalOptions = options.GlobalOptions;
            bool generateAttributes = true;

            // Try to read the build property
            if (globalOptions.TryGetValue("build_property.ThrowHelperExtensionsGenerateAttributes", out string? generateValue))
                generateAttributes = !string.Equals(generateValue, "false", StringComparison.OrdinalIgnoreCase);

            return generateAttributes;
        });
        var compilationWithConfig = context.CompilationProvider
            .Combine(buildOptions)
            .WithComparer(CompilationConfigComparer.Instance);
        // Attribute types to generate
        var attributeTypes = compilationWithConfig
            .SelectMany(static (config, token) => GetAttributeTypes(config, token))
            .Collect()
            .WithComparer(TypeNamesComparer.Instance)
            .SelectMany(static (types, _) => types);
        context.RegisterSourceOutput(attributeTypes, this.EmitAttributeType);

        // Generate ExceptionPolyfills
        context.RegisterSourceOutput(context.CompilationProvider, this.EmitPolyfillType);
    }

    /// <summary>
    /// Determines which attribute types need to be generated based on the compilation context and configuration.
    /// </summary>
    /// <param name="config">A tuple containing the compilation and whether to generate attributes.</param>
    /// <param name="token">Cancellation token to monitor for cancellation requests.</param>
    /// <returns>An immutable array of fully qualified type names that need to be generated.</returns>
    private static ImmutableArray<string> GetAttributeTypes((Compilation compilation, bool generateAttributes) config, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        // Skip if disabled on property or C# version is 13 or lower
        if (!config.generateAttributes || ((CSharpCompilation)config.compilation).LanguageVersion < MinimumRequiredLanguageVersion)
            return ImmutableArray<string>.Empty;

        return EmbeddedResources.Keys
            .Where(n => !IsTypeAlreadyExists(config.compilation, n, token))
            .ToImmutableArray();
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

        // lang=C#-test
        const string source = """
        // <auto-generated/>
        #pragma warning disable
        #nullable enable annotations

        #pragma warning disable CS0612
        #pragma warning disable CS0618
        #pragma warning disable CS0108
        #pragma warning disable CS0162
        #pragma warning disable CS0164
        #pragma warning disable CS0219
        #pragma warning disable CS8602
        #pragma warning disable CS8619
        #pragma warning disable CS8620
        #pragma warning disable CS8631
        #pragma warning disable CA1050

        // Licensed to the .NET Foundation under one or more agreements.
        // The .NET Foundation licenses this file to you under the MIT license.

        namespace Microsoft.CodeAnalysis;

        /// <summary>
        /// A special attribute recognized by Roslyn, that marks a type as "embedded", meaning it won't ever be visible from other assemblies.
        /// </summary>
        [global::System.AttributeUsage(global::System.AttributeTargets.All)]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        internal sealed partial class EmbeddedAttribute : global::System.Attribute
        { }
        """;
        context.AddSource("Microsoft.CodeAnalysis.EmbeddedAttribute.g.cs", source);
    }

    /// <summary>
    /// Emits a generated attribute source file into the compilation context.
    /// </summary>
    /// <param name="context">The source production context.</param>
    /// <param name="typeName">The fully qualified type name of the attribute.</param>
    private void EmitAttributeType(SourceProductionContext context, string typeName)
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

    /// <summary>
    /// Emits dynamically generated polyfill source files into the compilation context.
    /// </summary>
    /// <param name="context">The source production context.</param>
    /// <param name="compilation">The current compilation context.</param>
    private void EmitPolyfillType(SourceProductionContext context, Compilation compilation)
    {
        if (context.CancellationToken.IsCancellationRequested)
            return;

        // ExceptionPolyfills uses C# 14.0 features
        if (((CSharpCompilation)compilation).LanguageVersion < MinimumRequiredLanguageVersion)
            return;

        // Check if polyfill type already exists
        if (IsTypeAlreadyExists(compilation, "System.ExceptionPolyfills", context.CancellationToken))
            return;

        var parseOptions = compilation.SyntaxTrees.FirstOrDefault()?.Options as CSharpParseOptions;
        var targetFramework = GetTargetFrameworkFromSymbols(parseOptions?.PreprocessorSymbolNames ?? []);
        bool allowUnsafe = ((CSharpCompilation)compilation).Options.AllowUnsafe;
        string source = ExceptionPolyfillsBuilder.Generate(targetFramework, allowUnsafe);
        var sourceText = SourceText.From(source, Encoding.UTF8);
        context.AddSource("System.ExceptionPolyfills.g.cs", sourceText);
    }

    /// <summary>
    /// Detects the target framework version from preprocessor symbols.
    /// </summary>
    /// <param name="symbols">The preprocessor symbols.</param>
    /// <returns>The target framework version.</returns>
    internal static TargetFramework GetTargetFrameworkFromSymbols(IEnumerable<string> symbols)
    {
        if (symbols.Contains("NET8_0_OR_GREATER"))
            return TargetFramework.Net8OrGreater;
        if (symbols.Contains("NET7_0_OR_GREATER"))
            return TargetFramework.Net7;
        if (symbols.Contains("NET6_0_OR_GREATER"))
            return TargetFramework.Net6;

        return TargetFramework.PreNet6;
    }

    /// <summary>
    /// Comparer for compilation and build options tuple to optimize incremental generation.
    /// </summary>
    private sealed class CompilationConfigComparer : IEqualityComparer<(Compilation compilation, bool generateAttributes)>
    {
        public static readonly CompilationConfigComparer Instance = new();

        public bool Equals((Compilation compilation, bool generateAttributes) x, (Compilation compilation, bool generateAttributes) y) =>
            x.generateAttributes == y.generateAttributes && ReferenceEquals(x.compilation, y.compilation);

        public int GetHashCode((Compilation compilation, bool generateAttributes) obj)
        {
            unchecked
            {
                return (obj.compilation.GetHashCode() * 397) ^ obj.generateAttributes.GetHashCode();
            }
        }
    }

    /// <summary>
    /// Comparer for immutable arrays of type names to optimize incremental generation.
    /// </summary>
    private sealed class TypeNamesComparer : IEqualityComparer<ImmutableArray<string>>
    {
        public static readonly TypeNamesComparer Instance = new();

        public bool Equals(ImmutableArray<string> x, ImmutableArray<string> y) => x.SequenceEqual(y);

        public int GetHashCode(ImmutableArray<string> obj)
        {
            unchecked
            {
                int hash = obj.Length;
                foreach (string item in obj)
                    hash = (hash * 397) ^ item.GetHashCode();
                return hash;
            }
        }
    }
}

/// <summary>
/// Represents the target framework version.
/// </summary>
public enum TargetFramework
{
    /// <summary>Before .NET 6.0 (.NET 5.0 and earlier, .NET Standard 2.1 and earlier)</summary>
    PreNet6 = 0,
    /// <summary>.NET 6.0</summary>
    Net6 = 6,
    /// <summary>.NET 7.0</summary>
    Net7 = 7,
    /// <summary>.NET 8.0 or greater</summary>
    Net8OrGreater = 8,
}
