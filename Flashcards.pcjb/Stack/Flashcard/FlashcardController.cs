namespace Flashcards;

class FlashcardController
{
    private readonly Database database;
    private MainMenuController? mainMenuController;

    public FlashcardController(Database database)
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
        var view = new FlashcardMenuView(this, AppState.ActiveStack);
        view.SetMessage(message);
        view.Show();
    }

    public void ShowList()
    {
        ShowList(null);
    }

    public void ShowList(string? message)
    {
        var stack = AppState.ActiveStack;
        if (stack == null)
        {
            SelectStack();
            return;
        }
        var cards = database.ReadAllFlashcardsOfStack(stack.Id);
        List<FlashcardDto> cardDtos = new();
        foreach (Flashcard card in cards)
        {
            cardDtos.Add(new FlashcardDto(card.Id, card.Front, card.Back));
        }
        var view = new FlashcardListView(this, stack, cardDtos);
        view.SetMessage(message);
        view.Show();
    }

    public void ShowCreate()
    {
        ShowCreate(null);
    }

    public void ShowCreate(string? message)
    {
        var stack = AppState.ActiveStack;
        if (stack == null)
        {
            SelectStack();
            return;
        }
        var view = new FlashcardCreateView(this, stack);
        view.SetMessage(message);
        view.Show();
    }

    public void Create(Stack stack, string? front, string? back)
    {
        if (String.IsNullOrEmpty(front) || String.IsNullOrWhiteSpace(front)
        || String.IsNullOrEmpty(back) || String.IsNullOrWhiteSpace(back))
        {
            ShowMenu();
            return;
        }

        var cleanFront = front.Trim();
        var cleanBack = back.Trim();
        if (database.CreateFlashcard(stack.Id, cleanFront, cleanBack))
        {
            ShowMenu("OK - New flashcard created.");
        }
        else
        {
            ShowMenu("ERROR - Failed to save new flashcard.");
        }
    }

    public void ShowEdit()
    {
        // TODO
        ShowMenu();
    }

    public void ShowDelete()
    {
        // TODO
        ShowMenu();
    }

    public void SelectStack()
    {
        AppState.ActiveStack = null;
        if (mainMenuController == null)
        {
            throw new InvalidOperationException("Required MainMenuController missing.");
        }
        mainMenuController.ManageFlashcards();
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