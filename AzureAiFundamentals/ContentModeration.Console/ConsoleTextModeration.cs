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

namespace ContentModeration.Console
{
    public class ConsoleTextModeration
    {
        ILogger<ConsoleTextModeration> _logger;
        ContentSafetyClient _client;

        public ConsoleTextModeration(string key, string endpoint, ILogger<ConsoleTextModeration> logger)
        {
            _client = new ContentSafetyClient(new Uri(endpoint), new AzureKeyCredential(key));
            _logger = logger;
        }

        public void ModerateTextWithoutBlockList()
        {
            AnsiConsole.Clear();
            string input = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter the text you would like to moderate:")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid input[/]")
                    .Validate(text =>
                    {
                        return text.Length > 0;
                    }));
            AnsiConsole.WriteLine("Moderating text...");
            _logger.LogInformation($"Beginning text moderation. Input text: {input}");

            try
            {
                var response = _client.AnalyzeText(input);
                foreach (var value in response.Value.CategoriesAnalysis)
                {
                    AnsiConsole.WriteLine($"Category: {value.Category}, Severity: {value.Severity}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during text moderation: {ex.Message}");
                AnsiConsole.WriteLine($"An error occurred during text moderation: {ex.Message}");
            }
            


            AnsiConsole.WriteLine("Press any key to return to the main menu.");
        }
    }
}
