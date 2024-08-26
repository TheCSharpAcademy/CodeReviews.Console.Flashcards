using Spectre.Console;

namespace Flashcards;

public class FlashcardViews
{    
    static Color foregroundColor = ViewStyles.foregroundColor;
    
    internal static string FlashcardMenu()
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(foregroundColor)
                .Title($"\nSelect an [{foregroundColor}]option[/] from the menu:")
                .PageSize(7)
                .AddChoices(new[] {
                    "View All Flashcards in Stack", "View X Amount of Flashcards in Stack", "Create Flashcard in Stack", "Update Flashcard in Stack",
                    "Delete Flashcard in Stack", "Change Working Stack", "Return to Main Menu"
                }));

        return selected;
    }

    internal static string SelectColumnMenu()
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .HighlightStyle(foregroundColor)
                .Title($"\nWhich [{foregroundColor}]side[/] would you like to update?")
                .PageSize(3)
                .AddChoices(new[] {
                    "Front",  "Back"
                }));

        return selected;
    }
}