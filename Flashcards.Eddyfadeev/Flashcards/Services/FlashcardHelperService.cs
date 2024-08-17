using Flashcards.Enums;
using Flashcards.Interfaces.Models;
using Flashcards.Interfaces.Repositories;
using Flashcards.Interfaces.View.Factory;
using Spectre.Console;

namespace Flashcards.Services;

/// <summary>
/// Helper service for managing flashcards.
/// </summary>
internal static class FlashcardHelperService
{
    /// <summary>
    /// Gets the user input for a flashcard question.
    /// </summary>
    /// <returns>The user input for the flashcard question.</returns>
    internal static string GetQuestion()
    {
        AnsiConsole.MarkupLine(Messages.Messages.EnterFlashcardQuestionMessage);
        return AnsiConsole.Ask<string>(Messages.Messages.PromptArrow);
    }

    /// <summary>
    /// Gets the answer for a flashcard.
    /// </summary>
    /// <returns>The answer for the flashcard as a string.</returns>
    internal static string GetAnswer()
    {
        AnsiConsole.MarkupLine(Messages.Messages.EnterFlashcardAnswerMessage);
        return AnsiConsole.Ask<string>(Messages.Messages.PromptArrow);
    }

    /// <summary>
    /// Gets a flashcard from the flashcard menu.
    /// </summary>
    /// <param name="flashcardMenuCommandFactory">The factory for creating menu commands.</param>
    internal static void GetFlashcard(IMenuCommandFactory<FlashcardEntries> flashcardMenuCommandFactory)
    {
        var chooseCommand = flashcardMenuCommandFactory.Create(FlashcardEntries.ChooseFlashcard);
        chooseCommand.Execute();
    }

    /// <summary>
    /// Retrieves all flashcards from the flashcards repository.
    /// </summary>
    /// <param name="flashcardsRepository">The flashcards repository.</param>
    /// <returns>A list of flashcards.</returns>
    internal static List<IFlashcard> GetFlashcards(IFlashcardsRepository flashcardsRepository)
    {
        var flashcards = flashcardsRepository.GetAll().ToList();
        
        if (flashcards.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoEntriesFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return new List<IFlashcard>();
        }
        
        return flashcards;
    }
}