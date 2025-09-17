// See https://aka.ms/new-console-template for more information
using AzureAI.Core;
using AzureAI.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
});
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
ILogger<SecretService> secretLogger = loggerFactory.CreateLogger<SecretService>();


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
Console.WriteLine($"Key Name: {secretValues[keyName]}");
Console.WriteLine($"Endpoint Name: {secretValues[endpointName]}");
Console.ReadLine();

