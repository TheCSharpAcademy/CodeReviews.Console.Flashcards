using FlashCardsLibrary;
using FlashCardsLibrary.Models;
using Spectre.Console;

namespace FlashCards
{
    public static class ReportBuilder
    {
        public static void MakeReport()
        {
            var stacks = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Choose Stacks")
                    .Required() // Not required to have a favorite fruit
                    .PageSize(10)
                    .InstructionsText(
                        "Press [green]<enter>[/] to accept)")
                    .AddChoices(StackController.GetStackNames().Select(stack=>stack.Name)));

            // Write the selected fruits to the terminal
            foreach (var stack in stacks)
            {
                Table table = new Table();
                table.Title = new TableTitle(stack);
                table.Expand();
                table.AddColumns("[darkturquoise]Name[/]", "[darkturquoise]Date[/]", "[darkturquoise]Mark[/]");
                foreach (var session in StudySessionController.GetSessions(new Stack(stack)))
                {
                    table.AddRow($"{session.Name}",$"{session.Date.ToString("dd-MM-yyyy")}",$"{session.Answers}/{session.Total}");
                }
                AnsiConsole.Write(table);
            }
            Console.ReadLine();
            
        }
    }
}
