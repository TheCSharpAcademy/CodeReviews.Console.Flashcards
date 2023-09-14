namespace Flashcards;

class MainMenuController
{
    private StackController? stackController;

    public void SetStackController(StackController controller)
    {
        stackController = controller;
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
        stackController.ShowList();
    }

    public void Exit()
    {
        var view = new ExitView();
        view.Show();
    }
}