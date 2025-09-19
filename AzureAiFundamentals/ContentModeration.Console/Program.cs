// See https://aka.ms/new-console-template for more information
using AzureAI.Core;
using AzureAI.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Azure.AI.ContentSafety;
using Azure;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
ILogger<SecretService> secretLogger = loggerFactory.CreateLogger<SecretService>();


// Get the necessary secrets from Azure Key Vault for logging into the service
Console.WriteLine("Getting Key Vaule login inforation...");
string keyName = "ContentModerator-ApiKey";
string endpointName = "ContentModerator-Endpoint";
var secretService = new SecretService(secretLogger);
Dictionary<string, string> secretValues = new Dictionary<string, string>();
try
{
    secretValues = await secretService.GetSecretsAsync(new List<string> { keyName, endpointName });
}
catch (Exception ex)
{
    logger?.LogError($"Error retrieving secrets: {ex.Message}");
    throw;
}

Console.WriteLine("Key Vaule login inforation retrieved successfully.");

// Get the text that is to be evaluated
Console.WriteLine("Input text to be evaluated:");
string inputText = string.Empty;
while(string.IsNullOrWhiteSpace(inputText))
{
    inputText = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(inputText))
    {
        Console.WriteLine("Input cannot be empty. Please enter text to be evaluated:");
    }
}

// Evaluat the text and display the results
ContentSafetyClient client = new ContentSafetyClient(new Uri(secretValues[endpointName]), new Azure.AzureKeyCredential(secretValues[keyName]));

var request = new AnalyzeTextOptions(inputText);
Response<AnalyzeTextResult> response;
try
{
    response = await client.AnalyzeTextAsync(request);
}
catch(Exception ex)
{
       logger?.LogError($"Error analyzing text: {ex.Message}");
    throw;
}

Console.WriteLine("Content Safety Analysis Result:");
Console.WriteLine("Hate severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Hate)?.Severity ?? 0);
Console.WriteLine("SelfHarm severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.SelfHarm)?.Severity ?? 0);
Console.WriteLine("Sexual severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Sexual)?.Severity ?? 0);
Console.WriteLine("Violence severity: {0}", response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Violence)?.Severity ?? 0);
Console.ReadLine();


