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

var app = builder.Build();
app.Run();