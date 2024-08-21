using Flashcards.Eddyfadeev.Enums;
using Flashcards.Eddyfadeev.Exceptions;
using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
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
    private readonly IMenuHandler<ReportsMenuEntries> _reportsMenuHandler;
    
    private readonly IStudySessionsRepository _studySessionsRepository;
    private readonly IStacksRepository _stacksRepository;
    private readonly IFlashcardsRepository _flashcardsRepository;
    private readonly IEditableEntryHandler<IStack> _stackEntryHandler;

    public MainMenuEntriesInitializer(
        IMenuHandler<FlashcardEntries> flashcardsMenuHandler,
        IMenuHandler<StackMenuEntries> stacksMenuHandler,
        IMenuHandler<ReportsMenuEntries> reportsMenuHandler,
        IStudySessionsRepository studySessionsRepository, 
        IStacksRepository stacksRepository,
        IFlashcardsRepository flashcardsRepository,
        IEditableEntryHandler<IStack> stackEntryHandler
        )
    {
        _flashcardsMenuHandler = flashcardsMenuHandler;
        _stacksMenuHandler = stacksMenuHandler;
        _reportsMenuHandler = reportsMenuHandler;
        
        _studySessionsRepository = studySessionsRepository;
        _stacksRepository = stacksRepository;
        _flashcardsRepository = flashcardsRepository;
        _stackEntryHandler = stackEntryHandler;
    }

    public Dictionary<MainMenuEntries, Func<ICommand>> InitializeEntries(IMenuCommandFactory<MainMenuEntries> commandFactory) =>
        new()
        {
            { MainMenuEntries.StartStudySession, () => 
                new StartStudySession(
                    _stackEntryHandler, 
                    _stacksRepository, 
                    _studySessionsRepository, 
                    _flashcardsRepository
                    ) 
            },
            { MainMenuEntries.StudyHistory, () => new ShowStudyHistory(_reportsMenuHandler) },
            { MainMenuEntries.ManageStacks, () => new ManageStacks(_stacksMenuHandler) },
            { MainMenuEntries.ManageFlashcards, () => new ManageFlashcards(_flashcardsMenuHandler) },
            { MainMenuEntries.Exit, () => throw new ExitApplicationException()}
        };
}