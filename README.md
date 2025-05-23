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

### 3. Use throw helpers

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

    public Sample(string? value1)
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
- [`ArgumentNullException.ThrowIfNull(object?, string?)`](https://learn.microsoft.com/dotnet/api/system.argumentnullexception.throwifnull?view=net-6.0#System_ArgumentNullException_ThrowIfNull_System_Object_System_String_)
- [`ArgumentNullException.ThrowIfNull(void*, string?)`](https://learn.microsoft.com/dotnet/api/system.argumentnullexception.throwifnull?view=net-7.0#system-argumentnullexception-throwifnull(system-void*-system-string))
