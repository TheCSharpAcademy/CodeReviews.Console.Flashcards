namespace Flashcards;

class StudySessionController
{
    private readonly Database database;
    private MainMenuController? mainMenuController;

    public StudySessionController(Database database)
    {
        this.database = database;
    }

    public void SetMainMenuController(MainMenuController controller)
    {
        mainMenuController = controller;
    }

    public void ShowMenu()
    {
        ShowMenu(null);
    }

    public void ShowMenu(string? message)
    {
        var view = new StudySessionMenuView(this);
        view.SetMessage(message);
        view.Show();
    }

    public void StartNewSession()
    {
        // TODO
    }

    public void ShowSessionHistory()
    {
        // TODO
    }

    public void BackToMainMenu()
    {
        BackToMainMenu(null);
    }

    public void BackToMainMenu(string? message)
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.ShowMainMenu(message);
    }
}