using Flashcards.Enums;
using Flashcards.Interfaces.Repositories;
using Flashcards.Interfaces.View.Commands;
using Flashcards.Interfaces.View.Factory;
using Flashcards.Models.Entity;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.View.Commands.FlashcardsMenu;

/// <summary>
/// Represents a command for adding a flashcard.
/// </summary>
internal sealed class AddFlashcard : ICommand
{
    private readonly IFlashcardsRepository _flashcardsRepository;
    private readonly IStacksRepository _stacksRepository;
    private readonly IMenuCommandFactory<StackMenuEntries> _stackMenuCommandFactory;

    public AddFlashcard(
        IFlashcardsRepository flashcardsRepository, 
        IStacksRepository stacksRepository,
        IMenuCommandFactory<StackMenuEntries> stackMenuCommandFactory)
    {
        _flashcardsRepository = flashcardsRepository;
        _stacksRepository = stacksRepository;
        _stackMenuCommandFactory = stackMenuCommandFactory;
    }

    public void Execute()
    {
        var stack = StackChooserService.GetStacks(_stackMenuCommandFactory, _stacksRepository);
        
        GeneralHelperService.SetStackIdInRepository(_flashcardsRepository, stack);
        GeneralHelperService.SetStackNameInRepository(_flashcardsRepository, stack);
        
        var question = FlashcardHelperService.GetQuestion();
        var answer = FlashcardHelperService.GetAnswer();
        
        if (_flashcardsRepository.StackId is null)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoStackChosenMessage);
            GeneralHelperService.ShowContinueMessage();
            return;
        }
        
        var stackId = _flashcardsRepository.StackId;
        
        var flashcard = new Flashcard
        {
            Question = question,
            Answer = answer,
            StackId = stackId!.Value
        };
        
        _flashcardsRepository.Insert(flashcard);
    }
}