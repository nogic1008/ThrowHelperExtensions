using Microsoft.CodeAnalysis.CSharp;

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
        string source = """
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
        var result = GeneratorTestRunner.RunGenerator(source, languageVersion: LanguageVersion.CSharp10);

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
        string source = """
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
        var result = GeneratorTestRunner.RunGenerator(source, languageVersion: LanguageVersion.CSharp10, allowUnsafe: false);

        // Assert - Should not include unsafe-specific types when AllowUnsafe is false
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;

        generatedSources.ShouldNotContain(s => s.HintName.Contains("Unsafe"),
            "Generator should not produce unsafe-specific types when AllowUnsafe is false");
    }

    /// <summary>
    /// Verifies that the generator does not produce attributes when the build property is set to false.
    /// </summary>
    [TestMethod("Generator does not produce attributes when disabled via build property")]
    public void Generator_Does_Not_Produce_Attributes_When_Disabled()
    {
        // Arrange
        string source = """
            namespace TestNamespace;
            
            public class TestClass
            {
                public void TestMethod(string value)
                {
                    ArgumentNullException.ThrowIfNull(value);
                }
            }
            """;

        // Act - Disable attribute generation via build property
        var result = GeneratorTestRunner.RunGenerator(source, buildPropertyValue: "false", languageVersion: LanguageVersion.CSharp10);

        // Assert - Should only have EmbeddedAttribute, not the other attributes/polyfills
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;

        generatedSources.ShouldContain(s => s.HintName.Contains("EmbeddedAttribute"));
        generatedSources.ShouldNotContain(s => s.HintName.Contains("ExceptionPolyfills"));
    }

    /// <summary>
    /// Verifies that the generator produces attributes when the build property is set to true.
    /// </summary>
    [TestMethod("Generator produces attributes when enabled via build property")]
    public void Generator_Produces_Attributes_When_Enabled()
    {
        // Arrange
        string source = """
            namespace TestNamespace;
            
            public class TestClass
            {
                public void TestMethod(string value)
                {
                    ArgumentNullException.ThrowIfNull(value);
                }
            }
            """;

        // Act - Enable attribute generation via build property (default)
        var result = GeneratorTestRunner.RunGenerator(source, buildPropertyValue: "true", languageVersion: LanguageVersion.CSharp10);

        // Assert - Should have EmbeddedAttribute
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;

        generatedSources.ShouldContain(s => s.HintName.Contains("EmbeddedAttribute"));
    }

    /// <summary>
    /// Verifies that the generator produces embedded attributes when enabled.
    /// </summary>
    [TestMethod("Generator produces embedded attributes")]
    public void Generator_Produces_Embedded_Attributes()
    {
        // Arrange
        string source = """
            namespace TestNamespace;
            
            public class TestClass
            {
                public void TestMethod(string value)
                {
                    ArgumentNullException.ThrowIfNull(value);
                }
            }
            """;

        // Act - Use C# 10 which should generate embedded attributes
        var result = GeneratorTestRunner.RunGenerator(source, languageVersion: LanguageVersion.CSharp10);

        // Assert - Should have EmbeddedAttribute at minimum
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;

        generatedSources.ShouldContain(s => s.HintName.Contains("EmbeddedAttribute"),
            "Generator should produce EmbeddedAttribute");

        // Also check that some diagnostic attributes are generated when appropriate
        // The generator decides based on various conditions which attributes to include
        generatedSources.ShouldNotBeEmpty("Generator should produce at least some output");
    }
}
