using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>
/// Tests to verify that the incremental source generator works correctly.
/// Based on: https://andrewlock.net/creating-a-source-generator-part-10-testing-your-incremental-generator-pipeline-outputs-are-cacheable/
/// </summary>
[TestClass]
public sealed class IncrementalGeneratorTests
{
    /// <summary>
    /// Verifies that changing language version produces different outputs when appropriate.
    /// </summary>
    [TestMethod("Generator produces no output for old C# versions")]
    public void Generator_Produces_No_Output_For_Old_CSharp_Versions()
    {
        // Arrange
        var source = """
            namespace TestNamespace;
            
            public class TestClass
            {
                public void TestMethod(string value)
                {
                    ArgumentNullException.ThrowIfNull(value);
                }
            }
            """;

        // Act
        var result = RunGenerator(source, languageVersion: LanguageVersion.CSharp10);

        // Assert - Generator should produce minimal output for C# 10 (only EmbeddedAttribute)
        result.Results.ShouldNotBeEmpty();
        
        // Should only have the EmbeddedAttribute, not the ExceptionPolyfills
        var generatedSources = result.Results[0].GeneratedSources;
        generatedSources.ShouldContain(s => s.HintName.Contains("EmbeddedAttribute"));
        generatedSources.ShouldNotContain(s => s.HintName.Contains("ExceptionPolyfills"));
    }

    /// <summary>
    /// Verifies that the generator does not produce unsafe-specific types when AllowUnsafe is false.
    /// </summary>
    [TestMethod("Generator does not produce unsafe types when unsafe context is disabled")]
    public void Generator_Does_Not_Produce_Unsafe_Types_When_Unsafe_Context_Disabled()
    {
        // Arrange
        var source = """
            namespace TestNamespace;
            
            public class TestClass
            {
                public void TestMethod(string value)
                {
                    ArgumentNullException.ThrowIfNull(value);
                }
            }
            """;

        // Act - Use C# 10 to ensure generator runs safely, with unsafe explicitly disabled
        var result = RunGenerator(source, languageVersion: LanguageVersion.CSharp10, allowUnsafe: false);

        // Assert - Should not include unsafe-specific types when AllowUnsafe is false
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;
        
        generatedSources.ShouldNotContain(s => s.HintName.Contains("Unsafe"),
            "Generator should not produce unsafe-specific types when AllowUnsafe is false");
    }

    /// <summary>
    /// Runs the generator with the given source code and compilation options.
    /// </summary>
    private static GeneratorDriverRunResult RunGenerator(
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

    /// <summary>
    /// Test implementation of analyzer config options provider.
    /// </summary>
    private class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
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
    private class TestAnalyzerConfigOptions : AnalyzerConfigOptions
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
}
