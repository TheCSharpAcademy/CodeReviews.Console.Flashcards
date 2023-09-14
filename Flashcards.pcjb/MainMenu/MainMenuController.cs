namespace Flashcards;

class MainMenuController
{
    private StackController? stackController;
    private FlashcardController? flashcardController;

    public void SetStackController(StackController controller)
    {
        stackController = controller;
    }

    public void SetFlashcardController(FlashcardController controller)
    {
        flashcardController = controller;
    }

    public void ShowMainMenu()
    {
        ShowMainMenu(null);
    }

    public void ShowMainMenu(string? message)
    {
        var view = new MainMenuView(this);
        view.SetMessage(message);
        view.Show();
    }

    public void ManageStacks()
    {
        AppState.CurrentMode = AppState.Mode.ManageStacks;
        if (stackController == null)
        {
            throw new InvalidOperationException("Required StackController missing.");
        }
        stackController.ShowList();
    }

    public void ManageFlashcards()
    {
        AppState.CurrentMode = AppState.Mode.ManageFlashcards;
        if (stackController == null)
        {
            throw new InvalidOperationException("Required StackController missing.");
        }
        if (flashcardController == null)
        {
            throw new InvalidOperationException("Required FlashcardController missing.");
        }
        if (AppState.CurrentWorkingStack == null)
        {
            stackController.ShowList();
        } 
        else
        {
            flashcardController.ShowList(AppState.CurrentWorkingStack);
        }
    }

    public void Exit()
    {
        var view = new ExitView();
        view.Show();
    }
}