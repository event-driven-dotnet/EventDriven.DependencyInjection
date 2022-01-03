# EventDriven.DependencyInjection

Helper methods for configuring services with dependency injection.

#### Usage

- In a Web API project.

```csharp
builder.Services.AddAppSettings<MyAppSettings>(builder.Configuration);
```

- In a console project.

```csharp
var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var config = services.BuildServiceProvider()
            .GetRequiredService<IConfiguration>();
        services.AddAppSettings<MyAppSettings>(config);
    })
    .Build();
```