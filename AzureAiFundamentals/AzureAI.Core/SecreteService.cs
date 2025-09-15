using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Security.KeyVault.Secrets;
using AzureAI.Core.Interfaces;

namespace AzureAI.Core
{
    public class SecretService : IAzureSecretService
    {
        // Properties
        private static readonly string keyUrl = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_URL") ??
                                                throw new ArgumentNullException("KEY_VAULT_URL environment variable is not set.");

        public string ApiKey { get; set; }
        public string ApiEndpoint { get; set; }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var client = new SecretClient(new Uri(keyUrl), new Azure.Identity.DefaultAzureCredential());
            return (await client.GetSecretAsync(secretName)).Value.Value;
        }

        public async Task<Dictionary<string, string>> GetSecretsAsync(IEnumerable<string> secretNames)
        {
            var secrets = new Dictionary<string, string>();
            foreach (var name in secretNames)
            {
                var secretValue = await GetSecretAsync(name);
                if (secretValue != null && !secrets.ContainsKey(name))
                {
                    secrets.Add(name, secretValue);
                }
            }
            return secrets;
        }
    }
}
