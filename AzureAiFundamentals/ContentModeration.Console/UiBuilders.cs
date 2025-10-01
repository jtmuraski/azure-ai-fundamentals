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
            var table = new Table();

            table.AddRow("Severity Scores").Width(4);
            table.AddColumn("Hatred")
            table.AddColumn("Hatred");
            table.AddColumn("Hatred");
            table.AddColumn("Hatred");
            return;
        }
    }
}
