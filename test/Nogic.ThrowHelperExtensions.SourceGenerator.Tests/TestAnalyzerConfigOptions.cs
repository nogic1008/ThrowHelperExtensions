using Microsoft.CodeAnalysis.Diagnostics;

namespace Nogic.ThrowHelperExtensions.SourceGenerator.Tests;

/// <summary>
/// Test implementation of analyzer config options.
/// </summary>
internal class TestAnalyzerConfigOptions(Dictionary<string, string> options)
    : AnalyzerConfigOptions
{
#nullable disable warnings
    public override bool TryGetValue(string key, out string? value)
#nullable restore warnings
        => options.TryGetValue(key, out value);
}
