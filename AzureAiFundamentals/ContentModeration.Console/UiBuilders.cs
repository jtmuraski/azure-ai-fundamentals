using AzureAI.ContentModeration.Text.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace ContentModeration.Console
{
    public static class UiBuilders
    {
        public static void DisplayResults(TextModerationInstance instance)
        {
            var table = new Spectre.Console.Table();

            table.Title("Text Moderation Results");
            table.AddColumn("Hatred Score").Centered();
            table.AddColumn("Violence Score").Centered();
            table.AddColumn("Self Harm Score").Centered();
            table.AddColumn("Sexural Score").Centered();
            table.AddRow(instance.HateScore.ToString(), 
                instance.ViolenceScore.ToString(), 
                instance.SelfHarmScore.ToString(), 
                instance.SexualScore.ToString());

            AnsiConsole.Write(table);
            return;
        }
    }
}
