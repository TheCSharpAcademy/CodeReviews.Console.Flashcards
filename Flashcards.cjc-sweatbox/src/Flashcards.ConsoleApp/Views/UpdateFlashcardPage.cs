using Flashcards.ConsoleApp.Models;
using Flashcards.ConsoleApp.Services;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Views;

/// <summary>
/// Page which allows users to updated a flashcard.
/// </summary>
internal class UpdateFlashcardPage : BasePage
{
    #region Constants

    private const string PageTitle = "Update Flashcard";

    #endregion
    #region Methods

    internal static FlashcardDto? Show(FlashcardDto flashcard, string stackName)
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);

        // Show user the flashcard which is being updated.
        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Stack Name");
        table.AddColumn("Question");
        table.AddColumn("Answer");
        table.AddRow(flashcard.Id.ToString(), stackName, flashcard.Question, flashcard.Answer);

        var question = UserInputService.GetString($"Enter the updated [blue]question[/] for this flashcard, or [blue]0[/] to cancel updating: ");
        if (question == "0")
        {
            return null;
        }

        var answer = UserInputService.GetString($"Enter the updated [blue]answer[/] for this flashcard, or [blue]0[/] to cancel updating: ");
        if (answer == "0")
        {
            return null;
        }

        return new FlashcardDto(flashcard.StackId, question, answer);
    }

    #endregion
}
