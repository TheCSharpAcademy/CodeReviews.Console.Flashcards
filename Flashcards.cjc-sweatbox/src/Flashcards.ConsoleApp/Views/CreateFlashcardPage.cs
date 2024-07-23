using Flashcards.ConsoleApp.Services;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Views;

/// <summary>
/// Page which allows users to create a flashcard.
/// </summary>
internal class CreateFlashcardPage : BasePage
{
    #region Constants

    private const string PageTitle = "Create Flashcard";

    #endregion
    #region Methods - Internal

    internal static FlashcardDto? Show(int stackId)
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);
        
        var question = UserInputService.GetString($"Enter the [blue]question[/] for this flashcard, or [blue]0[/] to cancel creating: ");
        if (question == "0")
        {
            return null;
        }

        var answer = UserInputService.GetString($"Enter the [blue]answer[/] for this flashcard, or [blue]0[/] to cancel creating: ");
        if (answer == "0")
        {
            return null;
        }

        return new FlashcardDto(stackId, question, answer);
    }

    #endregion
}
