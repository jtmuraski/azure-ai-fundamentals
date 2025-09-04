namespace AzureAI.Core
{
    public interface IAzureConfig
    {
        string ApiEndpoint { get; set; }
        string ApiKey { get; set; }
    }
}