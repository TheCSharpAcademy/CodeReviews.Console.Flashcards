using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Services;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Commands.FlashcardsMenu;

/// <summary>
/// Represents a command that allows the user to choose a flashcard.
/// </summary>
internal sealed class ViewFlashcards : ICommand
{
    private readonly IFlashcardsRepository _flashcardsRepository;
    private readonly IStacksRepository _stacksRepository;
    private readonly IEditableEntryHandler<IFlashcard> _editableEntryHandler;
    private readonly IEditableEntryHandler<IStack> _stackEntryHandler;

    public ViewFlashcards(
        IFlashcardsRepository flashcardsRepository, 
        IStacksRepository stacksRepository,
        IEditableEntryHandler<IFlashcard> editableEntryHandler,
        IEditableEntryHandler<IStack> stackEntryHandler
        )
    {
        _flashcardsRepository = flashcardsRepository;
        _stacksRepository = stacksRepository;
        _editableEntryHandler = editableEntryHandler;
        _stackEntryHandler = stackEntryHandler;
    }

    public void Execute()
    {
        var stack = StackChooserService.GetStackFromUser(_stacksRepository, _stackEntryHandler);

        var flashcards = _flashcardsRepository.GetFlashcards(stack).ToList();

        if (flashcards.Count == 0)
        {
            AnsiConsole.MarkupLine(Messages.Messages.NoFlashcardsFoundMessage);
            GeneralHelperService.ShowContinueMessage();
            return;
        }
        _editableEntryHandler.HandleEditableEntry(flashcards);
    }
}