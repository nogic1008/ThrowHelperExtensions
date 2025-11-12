using Microsoft.CodeAnalysis.CSharp;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>
/// Tests to verify that the incremental source generator works correctly.
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
    /// Framework detection helper that determines what attributes should be generated
    /// based on the target framework being compiled against.
    /// </summary>
    private static class FrameworkCapabilities
    {
        private static readonly bool IsNetFramework = 
            System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription.StartsWith(".NET Framework");

        /// <summary>
        /// Determines if the current test environment expects CallerArgumentExpressionAttribute to be generated.
        /// .NET Framework and older .NET versions need this attribute generated.
        /// </summary>
        public static bool ShouldGenerateCallerArgumentExpressionAttribute() => IsNetFramework;

        /// <summary>
        /// Determines if nullability attributes should be generated.
        /// .NET Framework needs these attributes generated.
        /// </summary>
        public static bool ShouldGenerateNullabilityAttributes() => IsNetFramework;

        /// <summary>
        /// Determines if DoesNotReturn attributes should be generated.
        /// .NET Framework needs these attributes generated.
        /// </summary>
        public static bool ShouldGenerateDoesNotReturnAttributes() => IsNetFramework;
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
        
        // CallerArgumentExpressionAttribute should only be generated if not built into the framework
        if (FrameworkCapabilities.ShouldGenerateCallerArgumentExpressionAttribute())
        {
            generatedSources.ShouldContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"));
        }
        else
        {
            generatedSources.ShouldNotContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"));
        }
        
        // Check nullability attributes based on framework capabilities
        if (FrameworkCapabilities.ShouldGenerateNullabilityAttributes())
        {
            generatedSources.ShouldContain(s => s.HintName.Contains("NotNullAttribute"));
        }
        else
        {
            generatedSources.ShouldNotContain(s => s.HintName.Contains("NotNullAttribute"));
        }
        
        // Check DoesNotReturn attributes based on framework capabilities
        if (FrameworkCapabilities.ShouldGenerateDoesNotReturnAttributes())
        {
            generatedSources.ShouldContain(s => s.HintName.Contains("DoesNotReturnAttribute"));
        }
        else
        {
            generatedSources.ShouldNotContain(s => s.HintName.Contains("DoesNotReturnAttribute"));
        }
        
        // Unsafe types should NOT be generated when AllowUnsafe is false
        generatedSources.ShouldNotContain(s => s.HintName.Contains("Unsafe"));
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
        
        // CallerArgumentExpressionAttribute should only be generated if not built into the framework
        if (FrameworkCapabilities.ShouldGenerateCallerArgumentExpressionAttribute())
        {
            generatedSources.ShouldContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"));
        }
        else
        {
            generatedSources.ShouldNotContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"));
        }
        
        // Check nullability attributes based on framework capabilities
        if (FrameworkCapabilities.ShouldGenerateNullabilityAttributes())
        {
            generatedSources.ShouldContain(s => s.HintName.Contains("NotNullAttribute"));
        }
        else
        {
            generatedSources.ShouldNotContain(s => s.HintName.Contains("NotNullAttribute"));
        }
        
        // Check DoesNotReturn attributes based on framework capabilities
        if (FrameworkCapabilities.ShouldGenerateDoesNotReturnAttributes())
        {
            generatedSources.ShouldContain(s => s.HintName.Contains("DoesNotReturnAttribute"));
        }
        else
        {
            generatedSources.ShouldNotContain(s => s.HintName.Contains("DoesNotReturnAttribute"));
        }
        
        // Unsafe types SHOULD be generated when AllowUnsafe is true
        generatedSources.ShouldContain(s => s.HintName.Contains("Unsafe"));
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

    /// <summary>
    /// Verifies that attributes are generated appropriately based on the target framework.
    /// On newer frameworks (like .NET 9.0), some attributes are built-in and should not be generated.
    /// </summary>
    [TestMethod("Generator produces framework-appropriate attributes")]
    public void Generator_Produces_Framework_Appropriate_Attributes()
    {
        // Arrange - Act
        var result = GeneratorTestRunner.RunGenerator(Source);

        // Assert
        result.Results.ShouldNotBeEmpty();
        var generatedSources = result.Results[0].GeneratedSources;
        
        // EmbeddedAttribute should always be generated
        generatedSources.ShouldContain(s => s.HintName.Contains("EmbeddedAttribute"));
        
        // ExceptionPolyfills should be generated if language version is C# 14+
        generatedSources.ShouldContain(s => s.HintName.Contains("ExceptionPolyfills"));

        // Check CallerArgumentExpressionAttribute based on framework capabilities
        if (FrameworkCapabilities.ShouldGenerateCallerArgumentExpressionAttribute())
        {
            // On older frameworks, CallerArgumentExpressionAttribute should be generated
            generatedSources.ShouldContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"));
        }
        else
        {
            // On newer frameworks, CallerArgumentExpressionAttribute is built-in and should not be generated
            generatedSources.ShouldNotContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"));
        }

        // Check nullability attributes based on framework capabilities
        if (FrameworkCapabilities.ShouldGenerateNullabilityAttributes())
        {
            // On older frameworks, these attributes should be generated
            generatedSources.ShouldContain(s => s.HintName.Contains("NotNullAttribute"));
        }
        else
        {
            // On .NET Core 3.0+, nullability attributes are built-in
            generatedSources.ShouldNotContain(s => s.HintName.Contains("NotNullAttribute"));
        }

        // Check DoesNotReturn attributes based on framework capabilities
        if (FrameworkCapabilities.ShouldGenerateDoesNotReturnAttributes())
        {
            // On older frameworks, these attributes should be generated
            generatedSources.ShouldContain(s => s.HintName.Contains("DoesNotReturnAttribute"));
            generatedSources.ShouldContain(s => s.HintName.Contains("DoesNotReturnIfAttribute"));
        }
        else
        {
            // On .NET 5.0+, DoesNotReturn attributes are built-in
            generatedSources.ShouldNotContain(s => s.HintName.Contains("DoesNotReturnAttribute"));
            generatedSources.ShouldNotContain(s => s.HintName.Contains("DoesNotReturnIfAttribute"));
        }
    }
}
