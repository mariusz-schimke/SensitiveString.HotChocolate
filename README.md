# SensitiveString.HotChocolate
[![NuGet Version](http://img.shields.io/nuget/v/SensitiveString.HotChocolate.svg?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/SensitiveString.HotChocolate/) [![NuGet Downloads](https://img.shields.io/nuget/dt/SensitiveString.HotChocolate.svg?style=for-the-badge&logo=nuget)](https://www.nuget.org/packages/SensitiveString.HotChocolate/)

This package extends [SensitiveString](https://www.nuget.org/packages/SensitiveString) to integrate it with [HotChocolate](https://www.nuget.org/packages/HotChocolate/) so it can be used in inputs and responses.

## Example

```c#
using HotChocolate.Data;
using TextPrivacy.SensitiveString.HotChocolate;

var builder = WebApplication.CreateBuilder(args);

builder.Services
   .AddGraphQLServer()
   .AddSensitiveStringSupport()
   .AddFiltering(c => c.AddDefaults()
       .AddSensitiveStringSupport()
    )
   .AddSorting(c => c.AddDefaults()
       .AddSensitiveStringSupport()
    );
```