using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Security.KeyVault.Secrets;
using AzureAI.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace AzureAI.Core
{
    public class SecretService : IAzureSecretService
    {
        private static readonly string keyUrl = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_URL") ??
                                                throw new ArgumentNullException("KEY_VAULT_URL environment variable is not set.");

        private readonly ILogger<SecretService> _logger;
        private readonly SecretClient _client;

        // Main constructor for production
        public SecretService(ILogger<SecretService> logger)
            : this(logger, new SecretClient(new Uri(keyUrl), new Azure.Identity.DefaultAzureCredential()))
        {
        }

        // Constructor for testing (DI)
        public SecretService(ILogger<SecretService> logger, SecretClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            try
            {
                _logger.LogInformation($"Retrieving secret '{secretName}' from Key Vault.");

                var response = await _client.GetSecretAsync(secretName);

                _logger.LogInformation($"Successfully retrieved secret '{secretName}' from Key Vault.");
                return response.Value.Value;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error retrieving secret '{secretName}': {ex.Message}");
                throw;
            }
        }

        public async Task<Dictionary<string, string>> GetSecretsAsync(IEnumerable<string> secretNames)
        {
            var secrets = new Dictionary<string, string>();
            _logger.LogInformation($"There are {secretNames.Count()} secrets to retrieve from Key Vault.");
            foreach (var name in secretNames)
            {
                _logger.LogInformation($"Retrieving secret '{name}' from Key Vault.");
                var secretValue = await GetSecretAsync(name);
                if (secretValue != null && !secrets.ContainsKey(name))
                {
                    secrets.Add(name, secretValue);
                }
            }
            _logger.LogInformation($"Successfully retrieved {secrets.Count} secrets from Key Vault.");
            return secrets;
        }
    }
}
