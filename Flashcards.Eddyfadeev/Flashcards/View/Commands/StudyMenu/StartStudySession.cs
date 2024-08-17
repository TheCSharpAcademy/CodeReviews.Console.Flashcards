using Flashcards.Enums;
using Flashcards.Interfaces.Repositories;
using Flashcards.Interfaces.View.Commands;
using Flashcards.Interfaces.View.Factory;
using Flashcards.Models.Entity;
using Flashcards.Services;

namespace Flashcards.View.Commands.StudyMenu;

/// <summary>
/// Represents a command to start a study session.
/// </summary>
internal class StartStudySession : ICommand
{
    private readonly IMenuCommandFactory<StackMenuEntries> _stackMenuCommandFactory;
    private readonly IStacksRepository _stacksRepository;
    private readonly IStudySessionsRepository _studySessionsRepository;
    private readonly IFlashcardsRepository _flashcardsRepository;
    
    public StartStudySession(
        IMenuCommandFactory<StackMenuEntries> stackMenuCommandFactory,
        IStacksRepository stacksRepository,
        IStudySessionsRepository studySessionsRepository,
        IFlashcardsRepository flashcardsRepository
        )
    {
        _stackMenuCommandFactory = stackMenuCommandFactory;
        _stacksRepository = stacksRepository;
        _studySessionsRepository = studySessionsRepository;
        _flashcardsRepository = flashcardsRepository;
    }
    
    public void Execute()
    {
        var stack = StackChooserService.GetStacks(_stackMenuCommandFactory, _stacksRepository);
        
        StudySessionsHelperService.SetStackIdsInRepositories(stack, _flashcardsRepository, _studySessionsRepository);
        GeneralHelperService.SetStackNameInRepository(_studySessionsRepository, stack);
        
        var flashcards = FlashcardHelperService.GetFlashcards(_flashcardsRepository);
        
        if (flashcards.Count == 0)
        {
            return;
        }
        
        StudySession studySession = StudySessionsHelperService.CreateStudySession(flashcards, stack);
        
        var correctAnswers = StudySessionsHelperService.GetCorrectAnswers(flashcards);
        var currentTime = DateTime.Now;
        
        studySession.CorrectAnswers = correctAnswers;
        studySession.Time = currentTime - studySession.Date;

        _studySessionsRepository.Insert(studySession);
    }
}