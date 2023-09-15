namespace Flashcards;

class StudySessionController
{
    private readonly Database database;
    private MainMenuController? mainMenuController;
    private Stack? stack;
    private List<Flashcard>? cards;
    private int totalQuestions;
    private int correctAnswers;

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
        StartNewSession(null);
    }

    public void StartNewSession(Stack? selectedStack)
    {
        stack = selectedStack;
        if (stack == null)
        {
            SelectStack();
            return;
        }
        
        cards = database.ReadAllFlashcardsOfStack(stack.Id);
        if (cards == null || cards.Count < 1)
        {
            // TODO
            return;
        }
        totalQuestions = cards.Count;
        correctAnswers = 0;
        ShowNextRandomFlashcard();
    }

    public void SelectStack()
    {
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.StudySelectStack();
    }

    public void ShowNextRandomFlashcard()
    {
        if (stack == null)
        {
            throw new InvalidOperationException("stack was unexpectadly null");
        }

        if (cards == null)
        {
            throw new InvalidOperationException("cards was unexpectadly null");
        }

        if (cards.Count < 1)
        {
            FinishSession();
            return;
        }

        int answeredQuestions = totalQuestions - cards.Count;
        Random rand = new();
        int idx = rand.Next(0, cards.Count);
        var card = cards[idx];
        cards.RemoveAt(idx);

        var view = new StudySessionQuestionView(this, stack, card, answeredQuestions, totalQuestions);
        view.Show();
    }

    public void CheckAnswer(Flashcard card, string? answer)
    {
        if (card.Back.Equals(answer))
        {
            correctAnswers++;
        }
        ShowNextRandomFlashcard();
    }

    public void FinishSession()
    {
        if (stack == null)
        {
            throw new InvalidOperationException("stack was unexpectadly null");
        }

        var result = new StudySessionResult
        {
            TotalQuestions = totalQuestions,
            CorrectAnswers = correctAnswers
        };

        var session = new StudySession(stack.Id, DateTime.Now, result.ScorePercent);
        
        var view = new StudySessionResultView(this, stack, result);
        if (!database.CreateStudySession(session))
        {
            view.SetMessage("Error - Failed to save study session result.");
        }
        view.Show();
    }

    public void ShowSessionHistory()
    {
        var history = database.ReadStudySessionHistory();
        var view = new StudySessionHistoryView(this, history);
        view.Show();
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