using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>Executes generator tests.</summary>
internal static class GeneratorTestRunner
{
    private static readonly Compilation BaseCompilation;

    static GeneratorTestRunner()
    {
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(x => !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location))
            .Select(x => MetadataReference.CreateFromFile(x.Location))
            .ToList();

        // Add basic references
        references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

        // Try to add System.Runtime reference if available
        try
        {
            string systemRuntimePath = Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll");
            if (File.Exists(systemRuntimePath))
                references.Add(MetadataReference.CreateFromFile(systemRuntimePath));
        }
        catch
        {
            // Ignore if we can't find System.Runtime
        }

        var compilation = CSharpCompilation.Create("generator_test",
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        BaseCompilation = compilation;
    }

    public static GeneratorDriverRunResult RunGenerator(
        string source,
        LanguageVersion languageVersion = LanguageVersion.Preview,
        string generateAttributes = "true",
        bool allowUnsafe = false)
    {
        var parseOptions = new CSharpParseOptions(languageVersion);

        var driver = CSharpGeneratorDriver.Create(new ThrowHelperGenerator())
            .WithUpdatedParseOptions(parseOptions);

        var compilationOptions = allowUnsafe
            ? new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: allowUnsafe)
            : BaseCompilation.Options;

        var inputCompilation = BaseCompilation
            .WithOptions(compilationOptions)
            .AddSyntaxTrees(CSharpSyntaxTree.ParseText(source, parseOptions));

        // Add build property
        var optionsProvider = new TestAnalyzerConfigOptionsProvider(
            globalOptions: new Dictionary<string, string>
            {
                ["build_property.ThrowHelperExtensionsGenerateAttributes"] = generateAttributes
            });

        driver = driver.WithUpdatedAnalyzerConfigOptions(optionsProvider);

        // Run the generator and get the result
        driver = driver.RunGenerators(inputCompilation);

        return driver.GetRunResult();
    }
}
