using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Wikiled.Common.Utilities.Config;

public static class ConfigurationExtension
{
    public static T ExtractConfig<T>(this IConfiguration configuration, string name, IServiceCollection? service = null)
        where T : class, new()
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var config = new T();
        configuration.GetSection(name).Bind(config);
        if (config is IVerifiable verifiable &&
            !verifiable.Verify())
        {
            throw new Exception($"Configuration {name} failed validation");
        }

        service?.AddSingleton(config);
        return config;
    }
}
