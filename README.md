# ThrowHelperExtensions

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/nogic1008/ThrowHelperExtensions)](https://github.com/nogic1008/ThrowHelperExtensions/releases)
[![.NET CI](https://github.com/nogic1008/ThrowHelperExtensions/actions/workflows/dotnet-ci.yml/badge.svg)](https://github.com/nogic1008/ThrowHelperExtensions/actions/workflows/dotnet-ci.yml)
[![codecov](https://codecov.io/gh/nogic1008/ThrowHelperExtensions/graph/badge.svg?token=r9tFgjErCH)](https://codecov.io/gh/nogic1008/ThrowHelperExtensions)
[![CodeFactor](https://www.codefactor.io/repository/github/nogic1008/ThrowHelperExtensions/badge)](https://www.codefactor.io/repository/github/nogic1008/ThrowHelperExtensions)
[![License](https://img.shields.io/github/license/nogic1008/ThrowHelperExtensions)](LICENSE)

Use ThrowHelper methods (ex. `ArgumentException.ThrowIfNull`) on your elderly .NET project

## Usage

### 1. Set `LangVersion` to C# 14 (preview)

This package uses [Extension members](https://learn.microsoft.com/dotnet/csharp/whats-new/csharp-14#extension-members) feature on C# 14.

Please Add below on your `.csproj` or `Directory.Build.props`.

```xml
<PropertyGroup>
  <!-- 14.0 -->
  <LangVersion>preview</LangVersion>
</PropertyGroup>
```

### 2. Use throw helpers

```csharp
// Import namespace to use extension class
using Nogic.ThrowHelperExtensions;
// Or use `global using` on your `.csproj`
//  <ItemGroup>
//    <Using Include="Nogic.ThrowHelperExtensions" />
//  </ItemGroup>

namespace Samples;

public class Sample
{
    public string Value1 { get; }

    public Sample(string value1)
    {
        // On .NET 6.0 or higher, it calls `ArgumentNullException.ThrowIfNull` directly.
        // On others, it calls `ThrowHelperExtensions.ThrowIfNull` polyfill via extension members.
        ArgumentNullException.ThrowIfNull(value1);

        this.Value1 = value1;
    }
}
```

## API

- [`ArgumentException.ThrowIfNullOrEmpty(string?, string?)`](https://learn.microsoft.com/dotnet/api/system.argumentexception.throwifnullorempty?view=net-7.0)
- [`ArgumentException.ThrowIfNullOrWhiteSpace(string?, string?)`](https://learn.microsoft.com/dotnet/api/system.argumentexception.throwifnullorwhitespace?view=net-8.0)
- [`ArgumentNullException.ThrowIfNull(object?, string?)`](https://learn.microsoft.com/dotnet/api/system.argumentnullexception.throwifnull?view=net-6.0#System_ArgumentNullException_ThrowIfNull_System_Object_System_String_)
- [`ArgumentNullException.ThrowIfNull(void*, string?)`](https://learn.microsoft.com/dotnet/api/system.argumentnullexception.throwifnull?view=net-7.0#system-argumentnullexception-throwifnull(system-void*-system-string))
- [`ObjectDisposedException.ThrowIf(bool, object)`](https://learn.microsoft.com/dotnet/api/system.objectdisposedexception.throwif?view=net-7.0#system-objectdisposedexception-throwif(system-boolean-system-object))
- [`ObjectDisposedException.ThrowIf(bool, Type)`](https://learn.microsoft.com/dotnet/api/system.objectdisposedexception.throwif?view=net-7.0#system-objectdisposedexception-throwif(system-boolean-system-type))
- [`ArgumentOutOfRangeException.ThrowIfEqual<T>(T, T, string?) where T : IEquatable<T>?`](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifequal?view=net-8.0)
- [`ArgumentOutOfRangeException.ThrowIfGreaterThan<T>(T, T, string?) where T : IComparable<T>`](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifgreaterthan?view=net-8.0)
- [`ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual<T>(T, T, string?) where T : IComparable<T>`](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifgreaterthanorequal?view=net-8.0)
- [`ArgumentOutOfRangeException.ThrowIfLessThan<T>(T, T, string?) where T : IComparable<T>`](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwiflessthan?view=net-8.0)
- [`ArgumentOutOfRangeException.ThrowIfLessThanOrEqual<T>(T, T, string?) where T : IComparable<T>`](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwiflessthanorequal?view=net-8.0)
- [`ArgumentOutOfRangeException.ThrowIfNegative<T>(T, string?) where T : INumberBase<T>`](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifnegative?view=net-8.0) [^1]
- [`ArgumentOutOfRangeException.ThrowIfNegativeOrZero<T>(T, string?) where T : INumberBase<T>`](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifnegativeorzero?view=net-8.0) [^1]
- [`ArgumentOutOfRangeException.ThrowIfNotEqual<T>(T, T, string?) where T : IEquatable<T>`](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifnotequal?view=net-8.0)
- [`ArgumentOutOfRangeException.ThrowIfZero<T>(T, string?) where T : INumberBase<T>`](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception.throwifzero?view=net-8.0) [^1]

[^1]: [`INumberBase<T>`](https://learn.microsoft.com/dotnet/api/system.numerics.inumberbase-1?view=net-7.0) is not available in .NET 6.0 or below (including .NET Standard 2.0).
  So, this method only has overloads for the built-in numeric types. (`byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`, `decimal`, `nint`, `nuint`, `char`)
