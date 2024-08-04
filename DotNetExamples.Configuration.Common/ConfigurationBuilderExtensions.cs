using Microsoft.Extensions.Configuration;

namespace DotNetExamples.Configuration.Common;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddProviders<TAssembly>(
        this IConfigurationBuilder builder,
        string settingsBaseFileName,
        string environmentName)
        where TAssembly : class
    {
        // Create a temporary config, so we can get the configuration for Key Vault and Azure App Configuration.
        var tmpConfig = new ConfigurationBuilder()
            // Use the configuration as already provided by the run-time.
            .AddConfiguration(builder.Build())
            // Use configuration from the configuration files (custom extension).
            .AddConfigFiles(settingsBaseFileName, environmentName)
            // Add user secrets if running in the Development environment (custom extension).
            .AddUserSecretsForDevelopment<TAssembly>(environmentName)
            .Build();

        // Get the options to determine if we need to load config from Azure Key Vault and Azure App Configuration Store.
        var configurationProviderOptions =
            tmpConfig.GetConfigurationOrDefault<ConfigurationProviderOptions>("ConfigurationProviderOptions");

        // First add the configuration files (custom extension).
        builder.AddConfigFiles(settingsBaseFileName, environmentName);

        // Now add config from the Azure App Configuration or Azure Key Vault if needed.
        if (configurationProviderOptions.UseAppConfiguration || configurationProviderOptions.UseKeyVault)
        {
            var tokenCredential = configurationProviderOptions.AzureCredentialProvider.ToTokenCredential();

            if (configurationProviderOptions.UseAppConfiguration)
            {
                builder.AddAzureAppConfiguration(options =>
                {
                    options.Connect(configurationProviderOptions.AppConfigurationUri, tokenCredential);
                });
            }

            if (configurationProviderOptions.UseKeyVault)
            {
                builder.AddAzureKeyVault(configurationProviderOptions.KeyVaultUri, tokenCredential);
            }
        }

        // Finally, add user secrets if running in the Development environment.
        return builder.AddUserSecretsForDevelopment<TAssembly>(environmentName);
    }

    private static IConfigurationBuilder AddUserSecretsForDevelopment<TAssembly>(
        this IConfigurationBuilder builder,
        string environmentName)
        where TAssembly : class
    {
        if (environmentName == "Development")
        {
            // Only use the user secrets in the development environment, not in production.
            builder.AddUserSecrets<TAssembly>(optional: true, reloadOnChange: true);
        }

        return builder;
    }

    private static IConfigurationBuilder AddConfigFiles(
        this IConfigurationBuilder builder,
        string settingsBaseFileName,
        string environmentName)
    {
        // First add the mandatory base configuration file.
        // This file contains the basic settings, with defaults for development.
        builder.AddJsonFile($"{settingsBaseFileName}.json", optional: false, reloadOnChange: true);

        if (!string.IsNullOrWhiteSpace(environmentName))
        {
            // Add the optional configuration file for a specific environment.
            builder.AddJsonFile($"{settingsBaseFileName}.{environmentName}.json", optional: true, reloadOnChange: true);
        }

        return builder;
    }
}