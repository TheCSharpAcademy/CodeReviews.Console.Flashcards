using Spectre.Console;

namespace Flashcards.UndercoverDev.UserInteraction
{
    public class UserConsole : IUserConsole
    {
        public void MainMenu()
        {
            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Welcome. Select a [blue]function[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
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
        }

        public string GetUserInput()
        {
            var input = AnsiConsole.Ask<string>("[green]Input: [/]?");
            return input;
        }
    }
}