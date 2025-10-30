using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using AzureAiFundamentals;
using AzureAiFundamentals.Core.AzureUtils;
using Azure.Security.KeyVault.Secrets;
using Azure.AI.ContentSafety;
using Azure.Identity;
using Azure;
using AzureAiFundamentals.Core.Models;
using System.Collections;

namespace AiFundamentalFunctions;

public class TextModeration
{
    private readonly ILogger<TextModeration> _logger;
    private readonly string _keyUrl;
    private readonly SecretClient _secretClient;

    public TextModeration(ILogger<TextModeration> logger)
    {
        _logger = logger;
        _keyUrl = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_URL") ?? string.Empty;
        _secretClient = new SecretClient(new Uri(_keyUrl), new DefaultAzureCredential());


    }

    [Function("TextModeration")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        try
        {
            var moderationRequest = await ReadRequestAsync(req);
        }
        catch(Exception ex)
        {

        }


        try
        {
            SecretService secretService = new SecretService(new SecretClient(new Uri(_keyUrl), new Azure.Identity.DefaultAzureCredential()));
            var apiKey = await secretService.GetSecretAsync("ContentModerator-ApiKey");
            var endpoint = await secretService.GetSecretAsync("ContentModerator-Endpoint");

            ContentSafetyClient client = new ContentSafetyClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
            string inputText = "THis is a test of the Azure function"; //placeholder for input text
            var request = new AnalyzeTextOptions(inputText);
            Response<AnalyzeTextResult> response;
            response = client.AnalyzeText(request);
             return new OkObjectResult(response); //TODO: process response appropriately
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error during content moderation: {ex.Message}");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    private async Task<TextModerationRequest> ReadRequestAsync(HttpRequest req)
    {
        try
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            if(string.IsNullOrWhiteSpace(requestBody))
            {
                return null;
            }

            return JsonSerializer.Deserialize<TextModerationRequest>(requestBody, 
                                                                     new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch(Exception ex)
        {
            _logger.LogError($"Unexpected error when reading HTTP Request body: {ex.ToString()}");
            return null;
        }
    }
}