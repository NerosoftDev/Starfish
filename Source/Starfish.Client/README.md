# Starfish.Client

Starfish configuration provider implementation for [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/).

To use the Starfish configuration client, you need to deploy standalone Starfish backend service.

## How to?
The following example shows how to read application settings from the Starfish service using starfish client.

### Install NuGet package

NuGet package: 

| Package | Version| Downloads |
|--|--|--|
| [Starfish.Client](https://www.nuget.org/packages/Starfish.Client/) |![Nuget](https://img.shields.io/nuget/v/Starfish.Client?label=Starfish.Client)|![Nuget](https://img.shields.io/nuget/dt/Starfish.Client)|

```bash
Install-Package Starfish.Client
```

```powershell
dotnet add package Starfish.Client
```

```xml
<PackageReference Include="Starfish.Client" Version="$(StarfishVersion)" />
```

### Configurate Starfish service

in `appsettings.json`

```json
{
    "Starfish": {
        "Host": "http://localhost:5000",
        "App": "Starfish.Sample.Blazor",
        "Secret": "123456",
        "Env": "Development"
    }
}
```

### Add Starfish configuration provider

```csharp
// .NET 5
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            config.AddStarfish(ConfigurationClientOptions.Load(config));
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```
    
```csharp
// .NET 6 and above
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddStarfish(ConfigurationClientOptions.Load(builder.Configuration));
// ...
var app = builder.Build();
// ...
app.Run();
```