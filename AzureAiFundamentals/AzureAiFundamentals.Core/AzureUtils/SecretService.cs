using Azure.Security.KeyVault.Secrets;
using AzureAiFundamentals.Core.AzureUtils.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAiFundamentals.Core.AzureUtils
{
    public class SecretService : ISecretService
    {
        private static readonly string keyUrl = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_URL") ??
                                               throw new ArgumentNullException("KEY_VAULT_URL environment variable is not set.");

        private readonly SecretClient _client;

        // Constructor for testing (DI)
        public SecretService(SecretClient client)
        {
            _client = client;
        }


        // ---Async Methods---
        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {

                var response = await _client.GetSecretAsync(secretName);

                return response.Value.Value;
            }
            catch (Exception ex)
            {
                throw;
            }
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

        // --Sequential versions of the methods for scenarios where async is not needed--
        public string GetSecret(string secretName)
        {
            try
            { 
                var response = _client.GetSecret(secretName);

                return response.Value.Value;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public Dictionary<string, string> GetSecrets(IEnumerable<string> secretNames)
        {
            var secrets = new Dictionary<string, string>();
            foreach (var name in secretNames)
            {
                var secretValue = GetSecret(name);
                if (secretValue != null && !secrets.ContainsKey(name))
                {
                    secrets.Add(name, secretValue);
                }
            }
            return secrets;
        }
    }
}
