namespace DotNetExamples.Configuration.Common;

public class ConfigurationProviderOptions
{
    public Uri? KeyVaultUri { get; set; }

    public Uri? AppConfigurationUri { get; set; }

    public AzureCredentialProviderType AzureCredentialProvider { get; set; } = AzureCredentialProviderType.ManagedIdentityCredential;

    public bool UseKeyVault => KeyVaultUri != null;

    public bool UseAppConfiguration => AppConfigurationUri != null;
}