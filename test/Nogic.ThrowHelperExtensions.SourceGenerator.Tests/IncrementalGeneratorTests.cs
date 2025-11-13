using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>
/// Tests for <see cref="ThrowHelperGenerator"/>
/// </summary>
[TestClass]
public sealed class IncrementalGeneratorTests
{
    // lang=C#-test
    private const string Source = """
    namespace TestNamespace
    {
        public class TestClass
        {
        }
    }
    """;

    /// <summary>
    /// Determines if a type should be generated based on whether it's built into the framework.
    /// </summary>
    /// <param name="typeFullName">The full name of the type to check (including assembly name).</param>
    /// <returns>True if the type is not built into the framework and should be generated.</returns>
    private static void ShouldGeneratedOrBuiltIn(IEnumerable<GeneratedSourceResult> generatedSources, string typeFullName)
    {
        if (Type.GetType($"{typeFullName}, System.Runtime") is null)
            generatedSources.ShouldContain(s => s.HintName.Contains(typeFullName));
        else
            generatedSources.ShouldNotContain(s => s.HintName.Contains(typeFullName));
    }

    /// <summary>
    /// Verifies that the generator produces minimal output for old C# versions.
    /// </summary>
    [TestMethod("Generator produces no polyfills for old C# versions")]
    [DataRow(LanguageVersion.CSharp11)]
    [DataRow(LanguageVersion.CSharp10)]
    [DataRow(LanguageVersion.CSharp6)]
    public void Generator_Produces_No_Output_For_Old_CSharp_Versions(LanguageVersion languageVersion)
    {
        // Arrange - Act - Use the actual old language version to test the generator's language version check
        var result = GeneratorTestRunner.RunGenerator(Source, languageVersion);

        // Assert - Generator should produce minimal output for older C# versions (only EmbeddedAttribute)
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;

        // For old language versions (< C# 14), only EmbeddedAttribute should be generated
        generatedSources.ShouldContain(s => s.HintName.Contains("EmbeddedAttribute"));
        generatedSources.ShouldNotContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"));
        generatedSources.ShouldNotContain(s => s.HintName.Contains("ExceptionPolyfills"));
    }

    /// <summary>
    /// Verifies that the generator does not produce unsafe-specific types when AllowUnsafe is false.
    /// </summary>
    [TestMethod("Generator produces ExceptionPolyfills but not unsafe types when unsafe context is disabled")]
    public void Generator_Does_Not_Produce_Unsafe_Types_When_Unsafe_Context_Disabled()
    {
        // Arrange - Act
        var result = GeneratorTestRunner.RunGenerator(Source);

        // Assert
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;
        generatedSources.ShouldContain(s => s.HintName.Contains("EmbeddedAttribute"));
        generatedSources.ShouldContain(s => s.HintName.Contains("ExceptionPolyfills"));

        ShouldGeneratedOrBuiltIn(generatedSources, "System.Runtime.CompilerServices.CallerArgumentExpressionAttribute");
        ShouldGeneratedOrBuiltIn(generatedSources, "System.Diagnostics.CodeAnalysis.NotNullAttribute");
        ShouldGeneratedOrBuiltIn(generatedSources, "System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute");

        // Unsafe types should NOT be generated when AllowUnsafe is false
        generatedSources.ShouldNotContain(s => s.SourceText.ToString().Contains("unsafe"));
    }

    /// <summary>
    /// Verifies that the generator produces unsafe-specific types when AllowUnsafe is true.
    /// </summary>
    [TestMethod("Generator produces ExceptionPolyfills with unsafe types when unsafe context is enabled")]
    public void Generator_Produces_Unsafe_Types_When_Unsafe_Context_Enabled()
    {
        // Arrange - Act
        var result = GeneratorTestRunner.RunGenerator(Source, allowUnsafe: true);

        // Assert
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;
        generatedSources.ShouldContain(s => s.HintName.Contains("EmbeddedAttribute"));
        generatedSources.ShouldContain(s => s.HintName.Contains("ExceptionPolyfills"));

        ShouldGeneratedOrBuiltIn(generatedSources, "System.Runtime.CompilerServices.CallerArgumentExpressionAttribute");
        ShouldGeneratedOrBuiltIn(generatedSources, "System.Diagnostics.CodeAnalysis.NotNullAttribute");
        ShouldGeneratedOrBuiltIn(generatedSources, "System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute");

        // Unsafe types SHOULD be generated when AllowUnsafe is true
        // Check that the ExceptionPolyfills file contains unsafe code
        generatedSources.ShouldContain(s => s.SourceText.ToString().Contains("unsafe"));
    }

    /// <summary>
    /// Verifies that the generator does not produce attributes when the build property is set to false.
    /// </summary>
    [TestMethod("Generator does not produce attributes when disabled via build property")]
    public void Generator_Does_Not_Produce_Attributes_When_Disabled()
    {
        // Arrange - Act
        var result = GeneratorTestRunner.RunGenerator(Source, generateAttributes: "false");

        // Assert
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;

        // When attributes are disabled, only EmbeddedAttribute and ExceptionPolyfills should be generated
        // (as these are always needed for the generator itself)
        generatedSources.ShouldContain(s => s.HintName.Contains("EmbeddedAttribute"));
        generatedSources.ShouldContain(s => s.HintName.Contains("ExceptionPolyfills"));

        // But other attributes should not be generated
        generatedSources.ShouldNotContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"));
        generatedSources.ShouldNotContain(s => s.HintName.Contains("NotNullAttribute"));
        generatedSources.ShouldNotContain(s => s.HintName.Contains("DoesNotReturnAttribute"));
    }
}
