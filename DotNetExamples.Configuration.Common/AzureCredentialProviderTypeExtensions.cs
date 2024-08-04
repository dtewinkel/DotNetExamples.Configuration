using Azure.Core;
using Azure.Identity;

namespace DotNetExamples.Configuration.Common;

public static class AzureCredentialProviderTypeExtensions
{
    public static TokenCredential ToTokenCredential(this AzureCredentialProviderType azureCredentialProvider)
    {
        return azureCredentialProvider switch
        {
            AzureCredentialProviderType.DefaultAzureCredential => new DefaultAzureCredential(),
            AzureCredentialProviderType.AzureCliCredential => new AzureCliCredential(),
            AzureCredentialProviderType.EnvironmentCredential => new EnvironmentCredential(),
            AzureCredentialProviderType.ManagedIdentityCredential => new ManagedIdentityCredential(),
            AzureCredentialProviderType.VisualStudioCredential => new VisualStudioCredential(),
            _ => throw new InvalidOperationException(
                $"Value {azureCredentialProvider} is not supported for type {nameof(AzureCredentialProviderType)}.")
        };
    }
}