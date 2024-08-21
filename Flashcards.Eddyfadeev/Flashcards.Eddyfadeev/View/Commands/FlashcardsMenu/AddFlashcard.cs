using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Models.Entity;
using Flashcards.Eddyfadeev.Services;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Commands.FlashcardsMenu;

/// <summary>
/// Represents a command for adding a flashcard.
/// </summary>
internal sealed class AddFlashcard : ICommand
{
    private readonly IFlashcardsRepository _flashcardsRepository;
    private readonly IStacksRepository _stacksRepository;
    private readonly IEditableEntryHandler<IStack> _stackEntryHandler;

    public AddFlashcard(
        IFlashcardsRepository flashcardsRepository, 
        IStacksRepository stacksRepository,
        IEditableEntryHandler<IStack> stackEntryHandler)
    {
        _flashcardsRepository = flashcardsRepository;
        _stacksRepository = stacksRepository;
        _stackEntryHandler = stackEntryHandler;
    }

    public void Execute()
    {
        var stack = StackChooserService.GetStackFromUser(_stacksRepository, _stackEntryHandler);

        if (GeneralHelperService.CheckForNull(stack))
        {
            return;
        }
        
        var question = FlashcardHelperService.GetQuestion();
        var answer = FlashcardHelperService.GetAnswer();
        
        var flashcard = new Flashcard
        {
            Question = question,
            Answer = answer,
            StackId = stack.Id
        };
        
        var result = _flashcardsRepository.Insert(flashcard);
        
        AnsiConsole.MarkupLine(result > 0
            ? Messages.Messages.AddSuccessMessage
            : Messages.Messages.AddFailedMessage);
        GeneralHelperService.ShowContinueMessage();
    }
}