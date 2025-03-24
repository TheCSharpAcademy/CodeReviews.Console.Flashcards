using Flashcards.Models;
using Flashcards.Models.Dtos;
using Flashcards.Presenters.utils;
using Flashcards.Views;

namespace Flashcards.Presenters;

public class LearnPresenter(MainView view, MainRepository repository)
{
    private StackDto _selectedStack = new();
    
    public void Learn()
    {
        var stacks = repository.Stacks.GetAll();
        if (stacks.Count == 0)
        {
            view.ShowError("No stacks to learn from\n");
            view.PressKeyToContinue();

            return;
        }
        
        _selectedStack = view.SelectStack(stacks);
        var session = new LearningSession(view,
            _selectedStack.Id,
            () => repository.Flashcards.GetRandomFlashcardFromStack(_selectedStack.Id));
        var completedSession = session.StartSession();
        repository.Sessions.Insert(completedSession);
        view.ShowSuccess("Session saved successfully");
        view.PressKeyToContinue();
    }
}