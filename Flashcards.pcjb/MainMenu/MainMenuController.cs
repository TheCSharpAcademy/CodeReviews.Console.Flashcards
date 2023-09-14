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
        if (stackController == null)
        {
            throw new InvalidOperationException("Required StackController missing.");
        }
        stackController.ShowMenu();
    }

    public void ManageFlashcards()
    {
        ManageFlashcards(null);
    }

    public void ManageFlashcards(Stack? stack)
    {
        if (stack == null)
        {
            if (stackController == null)
            {
                throw new InvalidOperationException("Required StackController missing.");
            }
            stackController.ShowList(StackSelectionMode.ForFlashcards);
        }

        if (flashcardController == null)
        {
            throw new InvalidOperationException("Required FlashcardController missing.");
        }
        flashcardController.SetStack(stack);
        flashcardController.ShowMenu();
    }

    public void Exit()
    {
        var view = new ExitView();
        view.Show();
        Environment.Exit(0);
    }
}