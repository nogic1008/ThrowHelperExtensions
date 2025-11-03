using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>
/// Helper methods for running the source generator in tests.
/// </summary>
internal static class GeneratorTestRunner
{
    /// <summary>
    /// Runs the generator with the given source code and compilation options.
    /// </summary>
    public static GeneratorDriverRunResult RunGenerator(
        string source,
        string buildPropertyValue = "true",
        LanguageVersion languageVersion = LanguageVersion.Preview,
        bool allowUnsafe = false)
    {
        // Create compilation with proper references
        var parseOptions = CSharpParseOptions.Default.WithLanguageVersion(languageVersion);
        var syntaxTree = CSharpSyntaxTree.ParseText(source, parseOptions);

        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithAllowUnsafe(allowUnsafe);

        // Add basic references
        var references = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
        };

        // Try to add System.Runtime reference if available
        try
        {
            var systemRuntimePath = Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll");
            if (File.Exists(systemRuntimePath))
            {
                references.Add(MetadataReference.CreateFromFile(systemRuntimePath));
            }
        }
        catch
        {
            // Ignore if we can't find System.Runtime
        }

        var compilation = CSharpCompilation.Create(
            "TestAssembly",
            [syntaxTree],
            references,
            compilationOptions);

        // Create generator
        var generator = new ThrowHelperGenerator();

        // Create driver
        GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);

        // Add build property
        var optionsProvider = new TestAnalyzerConfigOptionsProvider(
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.ThrowHelperExtensionsGenerateAttributes"] = buildPropertyValue
            });

        driver = driver.WithUpdatedAnalyzerConfigOptions(optionsProvider);

        // Run the generator and get the result
        driver = driver.RunGenerators(compilation);
        return driver.GetRunResult();
    }
}

/// <summary>
/// Test implementation of analyzer config options provider.
/// </summary>
internal class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
{
    private readonly TestAnalyzerConfigOptions _globalOptions;

    public TestAnalyzerConfigOptionsProvider(Dictionary<string, string> globalOptions)
    {
        _globalOptions = new TestAnalyzerConfigOptions(globalOptions);
    }

    public override AnalyzerConfigOptions GlobalOptions => _globalOptions;

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => _globalOptions;

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => _globalOptions;
}

/// <summary>
/// Test implementation of analyzer config options.
/// </summary>
internal class TestAnalyzerConfigOptions : AnalyzerConfigOptions
{
    private readonly Dictionary<string, string> _options;

    public TestAnalyzerConfigOptions(Dictionary<string, string> options)
    {
        _options = options;
    }

    public override bool TryGetValue(string key, out string? value)
    {
        return _options.TryGetValue(key, out value);
    }
}
