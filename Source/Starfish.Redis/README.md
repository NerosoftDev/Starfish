# Microsoft.Extensions.Configuration.Redis

Redis configuration provider implementation for [Microsoft.Extensions.Configuration](https://www.nuget.org/packages/Microsoft.Extensions.Configuration/).

## Redis key/value

The Redis configuration provider requires a hash key in Redis. The following example shows how to store json settings to Redis hash structure.

> **Note:** The Redis configuration provider does not support nested values.

**Origin json data**

```json
{
  "Settings": {
    "Server": "localhost",
    "Database": "master",
    "Ports": [ "1433", "1434", "1435" ]
  }
}
```

The Redis key/value pair structure is shown below.

**Redis key:** 
`appsettings`

**Redis value:**
```text
Settings:Server = localhost
Settings:Database = master
Settings:Ports:0 = 1433
Settings:Ports:1 = 1434
Settings:Ports:2 = 1435
```

## How to?
The following example shows how to read application settings from the Redis.

### Install NuGet package

NuGet package: [Starfish.Redis](https://www.nuget.org/packages/Starfish.Redis/)

```bash
Install-Package Starfish.Redis
```

```powershell
dotnet add package Starfish.Redis
```

```xml
<PackageReference Include="Starfish.Redis" Version="$(StarfishVersion)" />
```

### Add Redis configuration provider

```cs
using System;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main()
    {
        IConfiguration config = new ConfigurationBuilder()
            .AddRedis("127.0.0.1:6379,ssl=False,allowAdmin=True,abortConnect=False,defaultDatabase=0,connectTimeout=500,connectRetry=3", "appsettings")
            .Build();

        // Get a configuration section
        IConfigurationSection section = config.GetSection("Settings");

        // Read simple values
        Console.WriteLine($"Server: {section["Server"]}");
        Console.WriteLine($"Database: {section["Database"]}");

        // Read a collection
        Console.WriteLine("Ports: ");
        IConfigurationSection ports = section.GetSection("Ports");

        foreach (IConfigurationSection child in ports.GetChildren())
        {
            Console.WriteLine(child.Value);
        }
    }
}
```

## Enable Redis Keyspace Notifications

The Redis configuration provider uses Redis keyspace notifications to invalidate the cache when the configuration changes. The Redis keyspace notifications are disabled by default. To enable the Redis keyspace notifications, set the `notify-keyspace-events` configuration option in the Redis configuration file to `AKE`.

1. Open terminal and run `redis-cli`.
2. Check the current configuration value use `CONFIG GET notify-keyspace-events`.
3. Set the `notify-keyspace-events` configuration option to `AKE` use `CONFIG SET notify-keyspace-events AKE`.