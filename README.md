# Kibii.Extensions.Configuration.InMemoryJson
This package allows you to use in-memory JSON strings as a configuration option for Microsoft.Extensions.Configuration (IConfigurationBuilder) classes.

# Usage

Simple, just like so:
```cs
IConfiguration configuration = new ConfigurationBuilder()
          .AddInMemoryJson("{\"MyValue\": 10}")
          .Build();
          
int value = configuration.GetValue<int>("MyValue");
```

# Download

You can either download the source or use the NuGet package available at https://www.nuget.org/packages/Kibii.Extensions.Configuration.InMemoryJson
