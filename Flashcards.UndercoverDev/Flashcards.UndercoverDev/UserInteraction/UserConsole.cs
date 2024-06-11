using System.Collections;
using Spectre.Console;

namespace Flashcards.UndercoverDev.UserInteraction
{
    public class UserConsole : IUserConsole
    {
        public string MainMenu()
        {
            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold]Welcome. Select a [blue]function[/]?[/]")
                    .PageSize(10)
                    .AddChoices([
                        "Add a Stack",
                        "Delete a Stack",
                        "Update a Stack",
                        "Add a Flashcard",
                        "Delete a Flashcard",
                        "Study Session",
                        "View Study Session by Stack",
                        "Average Score Yearly Report",
                        "Exit"
                        ]
                    )
            );

            return menu;
        }

        public string ShowMenu(string message, List<string> list)
        {
            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title(message)
                    .PageSize(10)
                    .AddChoices(list)
            );

            return menu;
        }

        public string GetUserInput(string message)
        {
            var input = AnsiConsole.Ask<string>($"[bold]{message}[/]");
            return input;
        }

        public void PrintMessage(string message, string color)
        {
            Thread.Sleep(500);
            switch (color)
            {
                case "green":
                    AnsiConsole.Write(new Markup($"\n[green]{message}[/]\n\n"));
                    break;
                case "red":
                    AnsiConsole.Write(new Markup($"\n[red]{message}[/]\n\n"));
                    break;
                default:
                    AnsiConsole.Write(new Markup($"\n[white]{message}[/]\n\n"));
                    break;
            }
            Thread.Sleep(1000);
        }

        public void WritTable(Table table)
        {
            AnsiConsole.Write(table);
        }

        public void WaitForAnyKey()
        {
            Console.ReadKey(true);
        }
    }
}