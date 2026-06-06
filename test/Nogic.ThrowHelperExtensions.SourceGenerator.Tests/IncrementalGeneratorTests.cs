using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>
/// Tests for <see cref="ThrowHelperGenerator"/>
/// </summary>
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
    private static async ValueTask VerifyTypeIsGeneratedOrBuiltInAsync(
        IEnumerable<GeneratedSourceResult> generatedSources,
        string typeFullName
    )
    {
        if (Type.GetType($"{typeFullName}, System.Runtime") is null)
        {
            await Assert.That(generatedSources).Contains(s => s.HintName.Contains(typeFullName));
        }
        else
        {
            await Assert
                .That(generatedSources)
                .DoesNotContain(s => s.HintName.Contains(typeFullName));
        }
    }

    /// <summary>
    /// Verifies that the generator produces minimal output for old C# versions.
    /// </summary>
    [Test]
    [DisplayName("Generator produces no polyfills and warns on $languageVersion")]
    [Arguments(LanguageVersion.CSharp11)]
    [Arguments(LanguageVersion.CSharp10)]
    [Arguments(LanguageVersion.CSharp6)]
    public async ValueTask Generator_Produces_No_Output_For_Old_CSharp_Versions(
        LanguageVersion languageVersion
    )
    {
        // Arrange - Act
        var result = GeneratorTestRunner.RunGenerator(Source, languageVersion);

        // Assert
        await Assert
            .That(result.Diagnostics)
            .Contains(d => d.Id == "THEX0001" && d.Severity == DiagnosticSeverity.Warning);

        await Assert.That(result.Results).IsNotEmpty();
        var generatedSources = result.Results[0].GeneratedSources;
        await Assert
            .That(generatedSources)
            .Contains(s => s.HintName.Contains("EmbeddedAttribute"))
            .And.DoesNotContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"))
            .And.DoesNotContain(s => s.HintName.Contains("ExceptionPolyfills"));
    }

    /// <summary>
    /// Verifies that the generator does not produce unsafe-specific types when AllowUnsafe is false.
    /// </summary>
    [Test]
    [DisplayName(
        "Generator produces ExceptionPolyfills but not unsafe types when unsafe context is disabled"
    )]
    public async ValueTask Generator_Does_Not_Produce_Unsafe_Types_When_Unsafe_Context_Disabled()
    {
        // Arrange - Act
        var result = GeneratorTestRunner.RunGenerator(Source);

        // Assert
        await Assert.That(result.Diagnostics).DoesNotContain(d => d.Id == "THEX0001");

        await Assert.That(result.Results).IsNotEmpty();
        var generatedSources = result.Results[0].GeneratedSources;
        await Assert
            .That(generatedSources)
            .Contains(s => s.HintName.Contains("EmbeddedAttribute"))
            .And.Contains(s => s.HintName.Contains("ExceptionPolyfills"));

        await VerifyTypeIsGeneratedOrBuiltInAsync(
            generatedSources,
            "System.Runtime.CompilerServices.CallerArgumentExpressionAttribute"
        );
        await VerifyTypeIsGeneratedOrBuiltInAsync(
            generatedSources,
            "System.Diagnostics.CodeAnalysis.NotNullAttribute"
        );
        await VerifyTypeIsGeneratedOrBuiltInAsync(
            generatedSources,
            "System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute"
        );

        // Unsafe types should NOT be generated when AllowUnsafe is false
        await Assert
            .That(generatedSources)
            .DoesNotContain(s => s.SourceText.ToString().Contains("unsafe"));
    }

    /// <summary>
    /// Verifies that the generator produces unsafe-specific types when AllowUnsafe is true.
    /// </summary>
    [Test]
    [DisplayName(
        "Generator produces ExceptionPolyfills with unsafe types when unsafe context is enabled"
    )]
    public async ValueTask Generator_Produces_Unsafe_Types_When_Unsafe_Context_Enabled()
    {
        // Arrange - Act
        var result = GeneratorTestRunner.RunGenerator(Source, allowUnsafe: true);

        // Assert
        await Assert.That(result.Diagnostics).DoesNotContain(d => d.Id == "THEX0001");

        await Assert.That(result.Results).IsNotEmpty();
        var generatedSources = result.Results[0].GeneratedSources;
        await Assert
            .That(generatedSources)
            .Contains(s => s.HintName.Contains("EmbeddedAttribute"))
            .And.Contains(s => s.HintName.Contains("ExceptionPolyfills"));

        await VerifyTypeIsGeneratedOrBuiltInAsync(
            generatedSources,
            "System.Runtime.CompilerServices.CallerArgumentExpressionAttribute"
        );
        await VerifyTypeIsGeneratedOrBuiltInAsync(
            generatedSources,
            "System.Diagnostics.CodeAnalysis.NotNullAttribute"
        );
        await VerifyTypeIsGeneratedOrBuiltInAsync(
            generatedSources,
            "System.Diagnostics.CodeAnalysis.DoesNotReturnAttribute"
        );

        // Unsafe types SHOULD be generated when AllowUnsafe is true
        // Check that the ExceptionPolyfills file contains unsafe code
        await Assert
            .That(generatedSources)
            .Contains(s => s.SourceText.ToString().Contains("unsafe"));
    }

    /// <summary>
    /// Verifies that the generator does not produce attributes when the build property is set to false.
    /// </summary>
    [Test]
    [DisplayName("Generator does not produce attributes when disabled via build property")]
    public async ValueTask Generator_Does_Not_Produce_Attributes_When_Disabled()
    {
        // Arrange - Act
        var result = GeneratorTestRunner.RunGenerator(Source, generateAttributes: "false");

        // Assert
        await Assert.That(result.Diagnostics).DoesNotContain(d => d.Id == "THEX0001");

        await Assert.That(result.Results).IsNotEmpty();
        var generatedSources = result.Results[0].GeneratedSources;
        await Assert
            .That(generatedSources)
            // When attributes are disabled, only EmbeddedAttribute and ExceptionPolyfills should be generated
            // (as these are always needed for the generator itself)
            .Contains(s => s.HintName.Contains("EmbeddedAttribute"))
            .And.Contains(s => s.HintName.Contains("ExceptionPolyfills"))
            // But other attributes should not be generated
            .And.DoesNotContain(s => s.HintName.Contains("CallerArgumentExpressionAttribute"))
            .And.DoesNotContain(s => s.HintName.Contains("NotNullAttribute"))
            .And.DoesNotContain(s => s.HintName.Contains("DoesNotReturnAttribute"));
    }

    /// <summary>
    /// Verifies that the generator does not generate ExceptionPolyfills when it already exists.
    /// </summary>
    [Test]
    [DisplayName("Generator does not produce ExceptionPolyfills when type already exists")]
    public async ValueTask Generator_Does_Not_Produce_ExceptionPolyfills_When_Type_Already_Exists()
    {
        // Arrange - Act
        // lang=C#-test
        const string sourceWithExistingType = """
            namespace System
            {
                public static class ExceptionPolyfills
                {
                    public static void ThrowIfNull(object argument) => throw new System.ArgumentNullException();
                }
            }
            namespace TestNamespace
            {
                public class TestClass
                {
                }
            }
            """;

        var result = GeneratorTestRunner.RunGenerator(sourceWithExistingType);

        // Assert
        await Assert.That(result.Results).IsNotEmpty();
        var generatedSources = result.Results[0].GeneratedSources;
        await Assert
            .That(generatedSources)
            // Should generate EmbeddedAttribute
            .Contains(s => s.HintName.Contains("EmbeddedAttribute"))
            // Should NOT generate ExceptionPolyfills because it already exists
            .And.DoesNotContain(s => s.HintName.Contains("ExceptionPolyfills"));
    }

    /// <summary>
    /// Verifies that GetTargetFrameworkFromSymbols correctly detects framework versions.
    /// </summary>
    [Test]
    [DisplayName(
        $"{nameof(ThrowHelperGenerator.GetTargetFrameworkFromSymbols)}($symbols) returns {nameof(TargetFramework)}.$expected"
    )]
    [Arguments((string[])[], TargetFramework.PreNet6)]
    [Arguments((string[])["NET5_0"], TargetFramework.PreNet6)]
    [Arguments((string[])["NETSTANDARD2_1"], TargetFramework.PreNet6)]
    [Arguments((string[])["NET6_0", "NET6_0_OR_GREATER"], TargetFramework.Net6)]
    [Arguments(
        (string[])["NET7_0", "NET6_0_OR_GREATER", "NET7_0_OR_GREATER"],
        TargetFramework.Net7
    )]
    [Arguments(
        (string[])["NET8_0", "NET6_0_OR_GREATER", "NET7_0_OR_GREATER", "NET8_0_OR_GREATER"],
        TargetFramework.Net8OrGreater
    )]
    [Arguments(
        (string[])
            [
                "NET9_0",
                "NET6_0_OR_GREATER",
                "NET7_0_OR_GREATER",
                "NET8_0_OR_GREATER",
                "NET9_0_OR_GREATER",
            ],
        TargetFramework.Net8OrGreater
    )]
    public async ValueTask GetTargetFrameworkFromSymbols_Detects_Framework_Version(
        string[] symbols,
        TargetFramework expected
    ) =>
        await Assert
            .That(ThrowHelperGenerator.GetTargetFrameworkFromSymbols(symbols))
            .EqualTo(expected);
}
