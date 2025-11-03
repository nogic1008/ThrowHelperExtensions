using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>
/// Test implementation of analyzer config options provider.
/// </summary>
internal class TestAnalyzerConfigOptionsProvider : AnalyzerConfigOptionsProvider
{
    private readonly TestAnalyzerConfigOptions globalOptions;

    public TestAnalyzerConfigOptionsProvider(Dictionary<string, string> globalOptions) => this.globalOptions = new TestAnalyzerConfigOptions(globalOptions);

    public override AnalyzerConfigOptions GlobalOptions => this.globalOptions;

    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => this.globalOptions;

    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => this.globalOptions;
}
