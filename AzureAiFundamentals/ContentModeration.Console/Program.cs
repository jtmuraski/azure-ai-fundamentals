// See https://aka.ms/new-console-template for more information
using AzureAI.Core;
using AzureAI.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Azure.AI.ContentSafety;
using Azure;
using Spectre.Console;
using Serilog;
using Figgle;
using Figgle.Fonts;
using ContentModeration.Console;
using static ContentModeration.Console.ConsoleTextModeration;

// ---Set up Logging---
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/;pg.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSerilog();
});
ILogger<Program> logger = loggerFactory.CreateLogger<Program>();
ILogger<SecretService> secretLogger = loggerFactory.CreateLogger<SecretService>();
ILogger<ConsoleTextModeration> cmLogger = loggerFactory.CreateLogger<ConsoleTextModeration>();

// ---Get Azure Secrets---
string keyName = "ContentModerator-ApiKey";
string endpointName = "ContentModerator-Endpoint";
var secretService = new SecretService(secretLogger);
Dictionary<string, string> secretValues = new Dictionary<string, string>();

// ---Global Variables and Properties---
ConsoleTextModeration? textModeration = null;

// --Set up Console---
FiggleFont figgle = FiggleFonts.Ogre;

AnsiConsole.Write(figgle.Render("Content Safety Example"));

AnsiConsole.Status()
    .Start("Starting Application...", ctx =>
    {
        ctx.Status("Setting up logs");
        ctx.Status("Logging into Azure");
        try
        {
            secretValues = secretService.GetSecrets(new List<string> { keyName, endpointName });
            textModeration = new ConsoleTextModeration(secretValues[keyName], secretValues[endpointName], cmLogger);
        }
        catch (Exception ex)
        {
            logger?.LogError($"Error retrieving secrets: {ex.Message}");
            throw;
        }
        ctx.Status("Logged in Successfully!");
        Thread.Sleep(1000);
    });
AnsiConsole.Clear();

bool continueApp = true;
while(continueApp)
{
    AnsiConsole.Write(figgle.Render("Content Safety Example"));
    AnsiConsole.WriteLine("Please choose an option below to continue.");

    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Choose an option:")
            .PageSize(10)
            .AddChoices(new[] {
                "1. Moderate Text",
                "9. Exit"
            }));
    switch (choice)
    {
        case "1. Moderate Text":
            textModeration.ModerateTextWithoutBlockList();
            break;
        case "9. Exit":
            continueApp = false;
            break;
    }
}












