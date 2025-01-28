using FunRun.Flashcards.Controller.Interfaces;
using FunRun.Flashcards.Data.Interfaces;
using FunRun.Flashcards.Data.Model;
using Microsoft.Extensions.Logging;

namespace FunRun.Flashcards.Controller;

public class CrudController : ICrudController
{
    private readonly IFlashcardsDataAccess _flashcard;
    private readonly IStackDataAccess _stack;
    private readonly ILogger<CrudController> _log;

    public CrudController(IFlashcardsDataAccess flashcard, IStackDataAccess stack, ILogger<CrudController> log)
    {
        _flashcard = flashcard;
        _stack = stack;
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

}
