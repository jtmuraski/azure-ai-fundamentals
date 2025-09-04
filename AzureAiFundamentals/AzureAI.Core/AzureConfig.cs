using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;

namespace AzureAI.Core
{
    public class AzureConfig : IAzureConfig
    {
        // Properties
        const string keyUrl = "https://kv-azure-fundys-jtm.vault.azure.net/";

        public string ApiKey { get; set; }
        public string ApiEndpoint { get; set; }

        public static async Task<AzureConfig> GetApiCredentialsAsync(string apiKeySecretName, string apiEndpointSecretName)
        {
            var config = new AzureConfig();

            config.ApiKey = await GetSecretFromKeyVaultAsync(apiKeySecretName);
            config.ApiEndpoint = await GetSecretFromKeyVaultAsync(apiEndpointSecretName);

            return config;
        }

        private static async Task<string?> GetSecretFromKeyVaultAsync(string apiKeySecretName)
        {
            var client = new SecretClient(new Uri(keyUrl), new Azure.Identity.DefaultAzureCredential());
            return (await client.GetSecretAsync(apiKeySecretName)).Value.Value;
        }
    }
}
