using Flashcards.Eddyfadeev.Interfaces.Handlers;
using Flashcards.Eddyfadeev.Interfaces.Models;
using Flashcards.Eddyfadeev.Interfaces.Repositories;
using Flashcards.Eddyfadeev.Interfaces.View.Commands;
using Flashcards.Eddyfadeev.Models.Entity;
using Flashcards.Eddyfadeev.Services;
using Spectre.Console;

namespace Flashcards.Eddyfadeev.View.Commands.MainMenu;

/// <summary>
/// Represents a command to start a study session.
/// </summary>
internal class StartStudySession : ICommand
{
    private readonly IEditableEntryHandler<IStack> _stackEntryHandler;
    private readonly IStacksRepository _stacksRepository;
    private readonly IStudySessionsRepository _studySessionsRepository;
    private readonly IFlashcardsRepository _flashcardsRepository;
    
    public StartStudySession(
        IEditableEntryHandler<IStack> stackEntryHandler,
        IStacksRepository stacksRepository,
        IStudySessionsRepository studySessionsRepository,
        IFlashcardsRepository flashcardsRepository
        )
    {
        _stackEntryHandler = stackEntryHandler;
        _stacksRepository = stacksRepository;
        _studySessionsRepository = studySessionsRepository;
        _flashcardsRepository = flashcardsRepository;
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
        
        StudySession studySession = StudySessionsHelperService.CreateStudySession(flashcards, stack);
        
        var correctAnswers = StudySessionsHelperService.GetCorrectAnswers(flashcards);
        var currentTime = DateTime.Now;
        
        studySession.CorrectAnswers = correctAnswers;
        studySession.Time = currentTime - studySession.Date;

        _studySessionsRepository.Insert(studySession);
    }
}