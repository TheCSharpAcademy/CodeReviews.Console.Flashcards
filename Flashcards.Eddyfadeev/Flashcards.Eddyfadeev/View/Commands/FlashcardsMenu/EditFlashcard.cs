using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;
using Flashcards.Eddyfadeev.Services;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Commands.FlashcardsMenu;

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