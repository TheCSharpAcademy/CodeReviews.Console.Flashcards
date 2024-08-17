using Flashcards.Enums;
using Flashcards.Interfaces.Repositories;
using Flashcards.Interfaces.View.Commands;
using Flashcards.Interfaces.View.Factory;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.View.Commands.FlashcardsMenu;

/// <summary>
/// Represents a command that allows editing a flashcard.
/// </summary>
internal sealed class EditFlashcard : ICommand
{
    private readonly IFlashcardsRepository _flashcardsRepository;
    private readonly IMenuCommandFactory<FlashcardEntries> _flashcardMenuCommandFactory;

    public EditFlashcard(
        IFlashcardsRepository flashcardsRepository, 
        IMenuCommandFactory<FlashcardEntries> flashcardMenuCommandFactory)
    {
        _flashcardsRepository = flashcardsRepository;
        _flashcardMenuCommandFactory = flashcardMenuCommandFactory;
    }

    public void Execute()
    {
        FlashcardHelperService.GetFlashcard(_flashcardMenuCommandFactory);
        
        var updatedQuestion = FlashcardHelperService.GetQuestion();
        var updatedAnswer = FlashcardHelperService.GetAnswer();
        
        _flashcardsRepository.SelectedEntry.Question = updatedQuestion;
        _flashcardsRepository.SelectedEntry.Answer = updatedAnswer;
        
        var result = _flashcardsRepository.Update();

        AnsiConsole.MarkupLine(result > 0
            ? Messages.Messages.UpdateSuccessMessage
            : Messages.Messages.UpdateFailedMessage);
        GeneralHelperService.ShowContinueMessage();
    }
}