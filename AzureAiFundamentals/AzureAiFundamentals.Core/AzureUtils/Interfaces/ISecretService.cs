namespace AzureAiFundamentals.Core.AzureUtils.Interfaces
{
    public interface ISecretService
    {
        string GetSecret(string secretName);
        Task<string> GetSecretAsync(string secretName);
        Dictionary<string, string> GetSecrets(IEnumerable<string> secretNames);
        Task<Dictionary<string, string>> GetSecretsAsync(IEnumerable<string> secretNames);
    }
}