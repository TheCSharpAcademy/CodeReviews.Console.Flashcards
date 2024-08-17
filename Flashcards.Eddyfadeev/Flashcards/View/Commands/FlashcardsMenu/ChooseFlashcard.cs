using Flashcards.Enums;
using Flashcards.Interfaces.Handlers;
using Flashcards.Interfaces.Models;
using Flashcards.Interfaces.Repositories;
using Flashcards.Interfaces.View.Commands;
using Flashcards.Interfaces.View.Factory;
using Flashcards.Services;
using Spectre.Console;

namespace Flashcards.View.Commands.FlashcardsMenu;

/// <summary>
/// Represents a command that allows the user to choose a flashcard.
/// </summary>
internal sealed class ChooseFlashcard : ICommand
{
    private readonly IFlashcardsRepository _flashcardsRepository;
    private readonly IStacksRepository _stacksRepository;
    private readonly IEditableEntryHandler<IFlashcard> _editableEntryHandler;
    private readonly IMenuCommandFactory<StackMenuEntries> _stackMenuCommandFactory;

    public ChooseFlashcard(
        IFlashcardsRepository flashcardsRepository, 
        IStacksRepository stacksRepository,
        IEditableEntryHandler<IFlashcard> editableEntryHandler,
        IMenuCommandFactory<StackMenuEntries> stackMenuCommandFactory)
    {
        _flashcardsRepository = flashcardsRepository;
        _stacksRepository = stacksRepository;
        _editableEntryHandler = editableEntryHandler;
        _stackMenuCommandFactory = stackMenuCommandFactory;
    }

    public void Execute()
    {
        var stack = StackChooserService.GetStacks(_stackMenuCommandFactory, _stacksRepository);
        
        GeneralHelperService.SetStackNameInRepository(_flashcardsRepository, stack);
        GeneralHelperService.SetStackIdInRepository(_flashcardsRepository, stack);
        
        var flashcards = _flashcardsRepository.GetAll().ToList();

        if (flashcards.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoFlashcardsFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return;
        }

        var userChoice = _editableEntryHandler.HandleEditableEntry(flashcards);
        
        if (GeneralHelperService.CheckForNull(userChoice))
        {
            return;
        }
        
        _flashcardsRepository.SelectedEntry = userChoice;
    }
}