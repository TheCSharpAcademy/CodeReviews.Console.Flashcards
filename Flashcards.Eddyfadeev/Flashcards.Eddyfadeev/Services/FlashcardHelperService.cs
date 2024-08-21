using Flashcards.Eddyfadeev.Extensions;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Models.Entity;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.Services;

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
    /// Retrieves a flashcard based on user input.
    /// </summary>
    /// <param name="stacksRepository">The repository for stacks.</param>
    /// <param name="flashcardsRepository">The repository for flashcards.</param>
    /// <param name="stackEntryHandler">The handler for stack entries.</param>
    /// <param name="flashcardEntryHandler">The handler for flashcard entries.</param>
    /// <returns>The flashcard that the user selects.</returns>
    internal static Flashcard GetFlashcardFromUser(
        IStacksRepository stacksRepository,
        IFlashcardsRepository flashcardsRepository,
        IEditableEntryHandler<IStack> stackEntryHandler,
        IEditableEntryHandler<IFlashcard> flashcardEntryHandler)
    {
        var stack = StackChooserService.GetStackFromUser(stacksRepository, stackEntryHandler);
        var flashcards = flashcardsRepository.GetFlashcards(stack).ToList();

        if (flashcards.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoEntriesFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return new Flashcard();
        }
        var userChoice = flashcardEntryHandler.HandleEditableEntry(flashcards)?.ToEntity();
        
        if (userChoice is null)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoFlashcardsFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return new Flashcard
            {
                Question = "Error getting Flashcard",
                Answer = "Error getting Flashcard"
            };
        }
        
        return userChoice;
    }
}