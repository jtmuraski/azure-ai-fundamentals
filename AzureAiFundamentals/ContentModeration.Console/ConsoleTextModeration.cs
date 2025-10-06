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
using AzureAI.ContentModeration.Text.Models;
using static ContentModeration.Console.UiBuilders;

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

        public void StartNewTextModeration()
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a text moderation option:")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Moderate Text with Block List",
                        "Moderate Text without Block List",
                        "Return to Main Menu"
                    }));
            switch (choice)
            {
                case "Moderate Text with Block List":
                    ModerateTextWithBlockList();
                    break;
                case "Moderate Text without Block List":
                    ModerateTextWithoutBlockList();
                    break;
                case "Return to Main Menu":
                    AnsiConsole.Clear();
                    return;
            }
        }

        private void ModerateTextWithBlockList()
        {
            throw new NotImplementedException();
        }

        public void ModerateTextWithoutBlockList()
        {
            TextModerationInstance instance = new TextModerationInstance();

            AnsiConsole.Clear();
            instance.TextToModerate = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter the text you would like to moderate:")
                    .PromptStyle("green")
                    .ValidationErrorMessage("[red]That's not a valid input[/]")
                    .Validate(text =>
                    {
                        return text.Length > 0;
                    }));
            AnsiConsole.WriteLine("Moderating text...");
            _logger.LogInformation($"Beginning text moderation. Input text: {instance.TextToModerate}");

            try
            {
                var response = _client.AnalyzeText(instance.TextToModerate);

                // Get the Hate Score
                var hateScore = response.Value.CategoriesAnalysis.FirstOrDefault(cat => cat.Category == TextCategory.Hate);
                instance.HateScore = hateScore.Severity;

                // Get the Vioolence Score
                var violenceScore = response.Value.CategoriesAnalysis.FirstOrDefault(cat => cat.Category == TextCategory.Violence);
                instance.ViolenceScore = violenceScore.Severity;

                // Get the SelfHarm Score
                var harmScore = response.Value.CategoriesAnalysis.FirstOrDefault(cat => cat.Category == TextCategory.SelfHarm);
                instance.SelfHarmScore = harmScore.Severity;

                // Get the Sexual Score
                var sexScore = response.Value.CategoriesAnalysis.FirstOrDefault(cat => cat.Category == TextCategory.Sexual);
                instance.SexualScore = sexScore.Severity;

                DisplayResults(instance);
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during text moderation: {ex.Message}");
                AnsiConsole.WriteLine($"An error occurred during text moderation: {ex.Message}");
            }
            


            AnsiConsole.Prompt<string>(
                new TextPrompt<string>("Press [green]Enter[/] to return to the main menu")
                    .AllowEmpty());
            AnsiConsole.Clear();
        }
    }
}
