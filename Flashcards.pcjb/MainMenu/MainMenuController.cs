namespace Flashcards;

class MainMenuController
{
    private StackController? stackController;
    private FlashcardController? flashcardController;
    private StudySessionController? studySessionController;
    private ReportController? reportController;

    public void SetStackController(StackController controller)
    {
        stackController = controller;
    }

    public void SetFlashcardController(FlashcardController controller)
    {
        flashcardController = controller;
    }

    public void SetStudySessionController(StudySessionController controller)
    {
        studySessionController = controller;
    }

    public void SetReportController(ReportController controller)
    {
        reportController = controller;
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

    public void Study()
    {
        if (studySessionController == null)
        {
            throw new InvalidOperationException("Required StudySessionController missing.");
        }
        studySessionController.ShowMenu();
    }

    public void Study(Stack selectedStack)
    {
        if (studySessionController == null)
        {
            throw new InvalidOperationException("Required StudySessionController missing.");
        }
        studySessionController.StartNewSession(selectedStack);
    }


    public void StudySelectStack()
    {
        if (stackController == null)
        {
            throw new InvalidOperationException("Required StackController missing.");
        }
        stackController.ShowList(StackSelectionMode.ForStudySession);
    }

    public void Reports()
    {
        if (reportController == null)
        {
            throw new InvalidOperationException("Required ReportController missing.");
        }
        reportController.ShowMenu();
    }

    public void Exit()
    {
        var view = new ExitView();
        view.Show();
        Environment.Exit(0);
    }
}