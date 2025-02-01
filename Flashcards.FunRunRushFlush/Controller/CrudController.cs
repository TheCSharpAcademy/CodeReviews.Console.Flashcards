using Flashcards.FunRunRushFlush.Controller.Interfaces;
using Flashcards.FunRunRushFlush.Data.Interfaces;
using Flashcards.FunRunRushFlush.Data.Model;
using Microsoft.Extensions.Logging;

namespace Flashcards.FunRunRushFlush.Controller;

public class CrudController : ICrudController
{
    private readonly IFlashcardsDataAccess _flashcard;
    private readonly IStackDataAccess _stack;
    private readonly IStudySessionDataAccess _studySession;


    private readonly ILogger<CrudController> _log;

    public CrudController(IFlashcardsDataAccess flashcard,
                          IStackDataAccess stack,
                          IStudySessionDataAccess studySession,
                          ILogger<CrudController> log)
    {
        _flashcard = flashcard;
        _stack = stack;
        _studySession = studySession;
        _log = log;
    }

    public List<Stack> ShowAllStacks()
    {
        try
        {
            return _stack.GetAllStacks();
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
            return new List<Stack>();
        }
    }

    public void CreateStack(Stack stack)
    {
        try
        {
            _stack.CreateStack(stack);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }

    public void UpdateStack(Stack stack)
    {
        try
        {
            _stack.UpdateStack(stack);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }

    public void DeleteStack(Stack stack)
    {
        try
        {
            _stack.DeleteStack(stack);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }

    public List<Flashcard> ShowAllFlashcardsOfSelectedStack(Stack stack)
    {
        try
        {
           return _flashcard.GetAllFlashcardsOfOneStack(stack);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
            return new List<Flashcard>();
        }
    }

    public void CreateFlashcard(Flashcard flashcard)
    {
        try
        {
            _flashcard.CreateFlashcard(flashcard);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }
    public void UpdateFlashcard(Flashcard flashcard)
    {
        try
        {
            _flashcard.UpdateFlashcard(flashcard);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }
    public void DeleteFlashcard(Flashcard flashcard)
    {
        try
        {
            _flashcard.DeleteFlashcard(flashcard);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }

    public List<StudySession> ShowAllStudySessions()
    {
        try
        {
            return _studySession.GetAllStudySessions();
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
            return new List<StudySession>();
        }
    }
    public void CreateStudySession(StudySession studySession)
    {
        try
        {
             _studySession.CreateStudySession(studySession);
        }
        catch (Exception ex)
        {
            _log.LogError(ex.Message, ex);
        }
    }

}
