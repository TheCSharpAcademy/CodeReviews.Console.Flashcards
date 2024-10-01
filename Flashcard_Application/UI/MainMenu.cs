using Flashcards.Validation;
using Spectre.Console;

namespace Flashcards.UI;

public class MainMenu
{
    public static void MainMenuPrompt()
    {
        var mainMenuSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Please select from the options below: [/]")
                .PageSize(10)
                .AddChoices(new[] {
            "Go to Study Area", "Stack & Card Management", "Close the Application",
                }));

        ValidationService.MainMenuInputValidation(mainMenuSelection);
    }
}
