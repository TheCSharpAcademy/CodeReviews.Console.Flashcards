using Flashcards.Validation;
using Spectre.Console;

namespace Flashcards.UI;

public class StackAndCardManagementMenu
{
    public static void StackAndCardManagementPrompt()
    {
        var stackAndCardMananementSelection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]\n\nPlease select from the options below: [/]")
                .PageSize(10)
                .AddChoices(new[] {
            "Create a Stack", "Delete a Stack", "Create a Card", "Delete a Card", "Return to Main Menu",
                }));

        ValidationService.StackCardManagementInputValidation(stackAndCardMananementSelection);
    }
}