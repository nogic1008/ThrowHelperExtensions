using Nogic.ThrowHelperExtensions;

namespace Samples;

public class Sample
{
    public string Value1 { get; }
    public int Value2 { get; }

    public Sample(string? value1, int value2)
    {
        ArgumentNullException.ThrowIfNull(value1);

        this.Value1 = value1;
        this.Value2 = value2;
    }
}
