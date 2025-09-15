namespace AzureAI.Core.Interfaces
{
    public interface IAzureConfig
    {
        string ApiEndpoint { get; set; }
        string ApiKey { get; set; }
    }
}