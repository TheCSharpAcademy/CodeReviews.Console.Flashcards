using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Exceptions;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;
using Flashcards.Eddyfadeev.View.Commands.FlashcardsMenu;

namespace Flashcards.Eddyfadeev.View.Factory.EntriesInitializers;

/// <summary>
/// Initializes the menu entries for the flashcards menu.
/// </summary>
internal class FlashcardsMenuEntriesInitializer : IMenuEntriesInitializer<FlashcardEntries>
{
    private readonly IFlashcardsRepository _flashcardsRepository;
    private readonly IStacksRepository _stacksRepository;
    private readonly IEditableEntryHandler<IFlashcard> _editableEntryHandler;
    private readonly IMenuCommandFactory<StackMenuEntries> _stackMenuCommandFactory;

    public FlashcardsMenuEntriesInitializer(
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

    public Dictionary<FlashcardEntries, Func<ICommand>> InitializeEntries(IMenuCommandFactory<FlashcardEntries> flashcardsMenuCommandFactory) =>
        new()
        {
            { FlashcardEntries.ChooseFlashcard, () => new ChooseFlashcard(_flashcardsRepository, _stacksRepository, _editableEntryHandler, _stackMenuCommandFactory) },
            { FlashcardEntries.AddFlashcard, () => new AddFlashcard(_flashcardsRepository, _stacksRepository,_stackMenuCommandFactory) },
            { FlashcardEntries.EditFlashcard, () => new EditFlashcard(_flashcardsRepository, flashcardsMenuCommandFactory) },
            { FlashcardEntries.DeleteFlashcard, () => new DeleteFlashcard(_flashcardsRepository, flashcardsMenuCommandFactory) },
            { FlashcardEntries.ReturnToMainMenu, () => throw new ReturnToMainMenuException()}
        };
}