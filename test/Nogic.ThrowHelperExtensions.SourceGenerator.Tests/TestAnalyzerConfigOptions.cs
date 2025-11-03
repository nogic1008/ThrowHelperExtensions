using Microsoft.CodeAnalysis.Diagnostics;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>
/// Test implementation of analyzer config options.
/// </summary>
internal class TestAnalyzerConfigOptions : AnalyzerConfigOptions
{
    private readonly Dictionary<string, string> options;

    public TestAnalyzerConfigOptions(Dictionary<string, string> options) => this.options = options;

    public override bool TryGetValue(string key, out string? value) => this.options.TryGetValue(key, out value);
}
