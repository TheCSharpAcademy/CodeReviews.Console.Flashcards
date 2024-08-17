using Flashcards.Enums;
using Flashcards.Exceptions;
using Flashcards.Interfaces.Report;
using Flashcards.Interfaces.Repositories;
using Flashcards.Interfaces.View.Commands;
using Flashcards.Interfaces.View.Factory;
using Flashcards.View.Commands.StudyMenu;

namespace Flashcards.View.Factory.EntriesInitializers;

/// <summary>
/// Initializes the menu entries for the study menu.
/// </summary>
internal class StudyMenuEntriesInitializer : IMenuEntriesInitializer<StudyMenuEntries>
{
    private readonly IStudySessionsRepository _studySessionsRepository;
    private readonly IStacksRepository _stacksRepository;
    private readonly IFlashcardsRepository _flashcardsRepository;
    private readonly IMenuCommandFactory<StackMenuEntries> _stackMenuCommandFactory;
    private readonly IReportGenerator _reportGenerator;
    
    public StudyMenuEntriesInitializer(
        IStudySessionsRepository studySessionsRepository, 
        IStacksRepository stacksRepository,
        IFlashcardsRepository flashcardsRepository,
        IMenuCommandFactory<StackMenuEntries> stackMenuCommandFactory,
        IReportGenerator reportGenerator
        )
    {
        _studySessionsRepository = studySessionsRepository;
        _stacksRepository = stacksRepository;
        _flashcardsRepository = flashcardsRepository;
        _stackMenuCommandFactory = stackMenuCommandFactory;
        _reportGenerator = reportGenerator;
    }
    
    public Dictionary<StudyMenuEntries, Func<ICommand>> InitializeEntries(
        IMenuCommandFactory<StudyMenuEntries> menuCommandFactory) =>
        new()
        {
            { StudyMenuEntries.StartStudySession, () => new StartStudySession(_stackMenuCommandFactory, _stacksRepository, _studySessionsRepository, _flashcardsRepository) },
            { StudyMenuEntries.StudyHistory, () => new ShowStudyHistory(_studySessionsRepository, _reportGenerator) },
            { StudyMenuEntries.ReturnToMainMenu, () => throw new ReturnToMainMenuException() }
        };
}