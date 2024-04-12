# SensitiveString.HotChocolate
[![NuGet Version](http://img.shields.io/nuget/v/SensitiveString.HotChocolate.svg?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/SensitiveString.HotChocolate/) [![NuGet Downloads](https://img.shields.io/nuget/dt/SensitiveString.HotChocolate.svg?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/SensitiveString.HotChocolate/)

This package extends [SensitiveString](https://www.nuget.org/packages/SensitiveString) to seamlessly integrate it with HotChocolate, so it can be used in inputs and responses.

## Example

```c#
using Microsoft.HotChocolate;
using TextPrivacy.SensitiveString.HotChocolate;

public class MyDbContext : DbContext
{
    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);
        builder.AddSensitiveStringSupport();
    }
}
```