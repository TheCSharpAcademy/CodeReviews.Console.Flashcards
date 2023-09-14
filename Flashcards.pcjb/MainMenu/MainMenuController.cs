namespace Flashcards;

class MainMenuController
{

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

    public void Exit()
    {
        var view = new ExitView();
        view.Show();
    }
}