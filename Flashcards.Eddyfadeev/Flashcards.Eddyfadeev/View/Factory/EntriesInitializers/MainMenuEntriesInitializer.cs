using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Exceptions;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Interfaces.View.Factory;
using Flashcards.Eddyfadeev.View.Commands.MainMenu;

namespace Flashcards.Eddyfadeev.View.Factory.EntriesInitializers;

/// <summary>
/// Initializes the menu entries for the main menu.
/// </summary>
internal class MainMenuEntriesInitializer : IMenuEntriesInitializer<MainMenuEntries>
{
    private readonly IMenuHandler<FlashcardEntries> _flashcardsMenuHandler;
    private readonly IMenuHandler<StackMenuEntries> _stacksMenuHandler;
    private readonly IMenuHandler<StudyMenuEntries> _studyMenuHandler;

    public MainMenuEntriesInitializer(
        IMenuHandler<FlashcardEntries> flashcardsMenuHandler,
        IMenuHandler<StackMenuEntries> stacksMenuHandler,
        IMenuHandler<StudyMenuEntries> studyMenuHandler
        )
    {
        _flashcardsMenuHandler = flashcardsMenuHandler;
        _stacksMenuHandler = stacksMenuHandler;
        _studyMenuHandler = studyMenuHandler;
    }

    public Dictionary<MainMenuEntries, Func<ICommand>> InitializeEntries(IMenuCommandFactory<MainMenuEntries> commandFactory) =>
        new()
        {
            { MainMenuEntries.StudyMenu, () => new OpenStudyMenu(_studyMenuHandler) },
            { MainMenuEntries.ManageStacks, () => new ManageStacks(_stacksMenuHandler) },
            { MainMenuEntries.ManageFlashcards, () => new ManageFlashcards(_flashcardsMenuHandler) },
            { MainMenuEntries.Exit, () => throw new ExitApplicationException()}
        };
}