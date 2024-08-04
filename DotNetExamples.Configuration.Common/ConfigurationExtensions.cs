using Microsoft.Extensions.Configuration;

namespace DotNetExamples.Configuration.Common;

public static class ConfigurationExtensions
{
    public static TConfiguration GetConfigurationOrDefault<TConfiguration>(this IConfiguration configuration, string sectionName) where TConfiguration: class, new()
    {
        return configuration.GetSection(sectionName).Get<TConfiguration>() ?? new TConfiguration();
    }
}