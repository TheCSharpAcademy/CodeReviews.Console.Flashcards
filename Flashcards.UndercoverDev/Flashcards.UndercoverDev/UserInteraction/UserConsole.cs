using Spectre.Console;

namespace Flashcards.UndercoverDev.UserInteraction
{
    public class UserConsole : IUserConsole
    {
        public string MainMenu()
        {
            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Welcome. Select a [blue]function[/]?")
                    .PageSize(10)
                    .AddChoices([
                        "Add a Stack",
                        "Delete a Stack",
                        "Add a Flashcard to a Stack",
                        "Delete a Flashcard from a Stack",
                        "Study Session",
                        "View Study Session by Stack",
                        "Average Score Yearly Report",
                        "Exit"
                        ]
                    )
            );

            return menu;
        }

        public string GetUserInput(string message)
        {
            var input = AnsiConsole.Ask<string>($"[green]{message}: [/]?");
            return input;
        }

        public void PrintMessage(string message, string color)
        {
            AnsiConsole.Write($"[{color}]{message}[/]");
        }
    }
}