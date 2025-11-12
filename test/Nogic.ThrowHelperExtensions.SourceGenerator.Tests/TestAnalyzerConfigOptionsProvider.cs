using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>
/// Test implementation of analyzer config options provider.
/// </summary>
internal class TestAnalyzerConfigOptionsProvider(Dictionary<string, string> globalOptions)
    : AnalyzerConfigOptionsProvider
{
    private readonly TestAnalyzerConfigOptions globalOptions = new(globalOptions);
    /// <inheritdoc />
    public override AnalyzerConfigOptions GlobalOptions => this.globalOptions;
    /// <inheritdoc />
    public override AnalyzerConfigOptions GetOptions(SyntaxTree tree) => this.globalOptions;
    /// <inheritdoc />
    public override AnalyzerConfigOptions GetOptions(AdditionalText textFile) => this.globalOptions;
}
