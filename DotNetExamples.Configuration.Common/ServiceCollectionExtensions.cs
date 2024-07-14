using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetExamples.Configuration.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDemoConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddSingleton(provider => provider
                .GetRequiredService<IConfiguration>()
                .GetSection(nameof(ConfigDataSingleton))
                .Get<ConfigDataSingleton>() ?? new ConfigDataSingleton { FromSource = "Default" })

            .AddScoped(provider => provider
                .GetRequiredService<IConfiguration>()
                .GetSection(nameof(ConfigDataScoped))
                .Get<ConfigDataScoped>() ?? new ConfigDataScoped { FromSource = "Default" })

            .Configure<ConfigDataOptions>(configuration.GetSection(nameof(ConfigDataOptions)))
            .Configure<ConfigDataOptionsSnapshot>(configuration.GetSection(nameof(ConfigDataOptionsSnapshot)))
            .Configure<ConfigDataOptionsMonitor>(configuration.GetSection(nameof(ConfigDataOptionsMonitor)));

        services.AddOptions<ConfigDataOptionsValidation>()
            .BindConfiguration(nameof(ConfigDataOptionsValidation))
            .ValidateDataAnnotations();

        services.AddOptionsWithValidateOnStart<ConfigDataOptionsValidationStartup>()
            .BindConfiguration(nameof(ConfigDataOptionsValidationStartup))
            .ValidateDataAnnotations();

        return services;
    }
}