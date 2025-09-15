using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAI.Core.Interfaces
{
    public interface IAzureSecretService
    {
        Task<string> GetSecretAsync(string secretName);
        Task<Dictionary<string, string>> GetSecretsAsync(IEnumerable<string> secretNames);
    }
}
