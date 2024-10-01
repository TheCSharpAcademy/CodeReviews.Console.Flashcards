using Flashcards.Validation;
using Spectre.Console;

namespace Flashcards.UI;

public class StudyAreaMenu
{
    public static void StudyAreaPrompt()
    {
        var studyAreaSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Please select from the options below: [/]")
                .PageSize(10)
                .AddChoices(new[] {
            "Choose a Stack to study", "View study History", "Return to Main Menu",
                }));

        ValidationService.StudyAreaInputValidation(studyAreaSelection);
    }
}
