using Flashcards.ConsoleApp.Enums;
using Flashcards.ConsoleApp.Models;
using Flashcards.Controllers;
using Flashcards.Models;
using Spectre.Console;

namespace Flashcards.ConsoleApp.Views;

/// <summary>
/// Page which allows users to manage a stack of flashcards.
/// </summary>
internal class ManageFlashcardsPage : BasePage
{
    #region Constants

    private const string PageTitle = "Manage Flashcards";

    #endregion
    #region Fields

    private readonly FlashcardController _flashcardController;
    private readonly StackDto _stack;

    #endregion
    #region Constructors

    public ManageFlashcardsPage(FlashcardController flashcardController, StackDto stack)
    {
        _flashcardController = flashcardController;
        _stack = stack;
    }

    #endregion
    #region Properties
    
    internal static IEnumerable<UserChoice> PageChoices
    {
        get
        {
            return
            [
                new(1, "Add flashcard"),
                new(2, "Update flashcard"),
                new(3, "Delete flashcard"),
                new(0, "Close page")
            ];
        }
    }

    #endregion
    #region Methods - Internal

    internal void Show()
    {
        var status = PageStatus.Opened;

        while (status != PageStatus.Closed)
        {
            AnsiConsole.Clear();

            WriteHeader(PageTitle);

            var option = AnsiConsole.Prompt(
                new SelectionPrompt<UserChoice>()
                .Title(PromptTitle)
                .AddChoices(PageChoices)
                .UseConverter(c => c.Name!)
                );

            status = PerformOption(option);
        }
    }

    #endregion
    #region Methods - Private

    private PageStatus PerformOption(UserChoice option)
    {
        switch (option.Id)
        {
            case 1:

                // Add flashcard.
                AddFlashcard();
                break;

            case 2:

                // Update flashcard.
                UpdateFlashcard();
                break;

            case 3:

                // Delete flashcard.
                DeleteFlashcard();
                break;

            default:

                // Close page.
                return PageStatus.Closed;
        }

        return PageStatus.Opened;
    }

    private void AddFlashcard()
    {
        // Get new flashcard.
        FlashcardDto? flashcard = CreateFlashcardPage.Show(_stack.Id);
        if (flashcard == null)
        {
            return;
        }

        // Commit to database.
        _flashcardController.AddFlashcard(flashcard.StackId, flashcard.Question, flashcard.Answer);

        // Show message.
        MessagePage.Show("Create Flashcard", "Flashcard created successfully.");
    }

    private void UpdateFlashcard()
    {
        // Get raw data.
        var flashcards = _flashcardController.GetFlashcards(_stack.Id);

        // Get flashcard to manage, or stop.
        FlashcardDto? flashcard = SelectFlashcardPage.Show(flashcards);
        if (flashcard == null)
        {
            return;
        }

        // Get updated flashcard.
        FlashcardDto? updatedFlashcard = UpdateFlashcardPage.Show(flashcard, _stack.Name);
        if (updatedFlashcard == null)
        {
            return;
        }

        // Commit to database.
        _flashcardController.SetFlashcard(flashcard.Id, updatedFlashcard.Question, updatedFlashcard.Answer);

        // Show message.
        MessagePage.Show("Update Flashcard", "Flashcard updated successfully.");
    }

    private void DeleteFlashcard()
    {
        // Get raw data.
        var flashcards = _flashcardController.GetFlashcards(_stack.Id);

        // Get flashcard to manage, or stop.
        FlashcardDto? flashcard = SelectFlashcardPage.Show(flashcards);
        if (flashcard == null)
        {
            return;
        }

        // Commit to database.
        _flashcardController.DeleteFlashcard(flashcard.Id);

        // Show message.
        MessagePage.Show("Delete Flashcard", "Flashcard deleted successfully.");
    }

    #endregion
}
