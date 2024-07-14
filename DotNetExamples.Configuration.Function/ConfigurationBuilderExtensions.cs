using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Configuration;

namespace DotNetExamples.Configuration.Function;
public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddProviders(this IConfigurationBuilder builder)
    {
        builder
            .AddJsonFile("functionSettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>(optional: true, reloadOnChange: true)
            ;

        var configuration = builder.Build();

        var demoOptions = configuration.GetSection("DemoOptions").Get<DemoOptions>() ?? new DemoOptions();

        var tokenCredential = demoOptions.AzureCredentialProvider.ToTokenCredential();
        if (demoOptions.KeyVaultUri != null)
        {
            builder.AddAzureKeyVault(demoOptions.KeyVaultUri, tokenCredential);
        }

        if (demoOptions.AppConfigurationUri != null)
        {
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(demoOptions.AppConfigurationUri, tokenCredential);
            });
        }

        return builder;
    }
}

public enum AzureCredentialProviderType
{
    AzureCliCredential,
    AzureDeveloperCliCredential,
    DefaultAzureCredential,
    DeviceCodeCredential,
    EnvironmentCredential,
    ManagedIdentityCredential,
    VisualStudioCodeCredential,
    VisualStudioCredential,
    WorkloadIdentityCredential
}

public class DemoOptions
{
    public Uri? KeyVaultUri { get; set; }

    public Uri? AppConfigurationUri { get; set; }

    public AzureCredentialProviderType AzureCredentialProvider { get; set; } = AzureCredentialProviderType.DefaultAzureCredential;
}

public static class AzureCredentialProviderExtensions
{
    public static TokenCredential ToTokenCredential(this AzureCredentialProviderType azureCredentialProvider)
    {
        return azureCredentialProvider switch
        {
            AzureCredentialProviderType.DefaultAzureCredential => new DefaultAzureCredential(),
            AzureCredentialProviderType.AzureCliCredential => new AzureCliCredential(),
            AzureCredentialProviderType.AzureDeveloperCliCredential => new AzureDeveloperCliCredential(),
            AzureCredentialProviderType.DeviceCodeCredential => new DeviceCodeCredential(),
            AzureCredentialProviderType.EnvironmentCredential => new EnvironmentCredential(),
            AzureCredentialProviderType.ManagedIdentityCredential => new ManagedIdentityCredential(),
            AzureCredentialProviderType.VisualStudioCodeCredential => new VisualStudioCodeCredential(),
            AzureCredentialProviderType.VisualStudioCredential => new VisualStudioCredential(),
            AzureCredentialProviderType.WorkloadIdentityCredential => new WorkloadIdentityCredential(),
            _ => throw new InvalidOperationException(
                $"Value {azureCredentialProvider} is not supported for type {nameof(AzureCredentialProviderType)}.")
        };
    }
}