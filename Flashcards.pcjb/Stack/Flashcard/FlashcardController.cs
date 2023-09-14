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

    public void ShowList(FlashcardSelectionMode mode)
    {
        ShowList(mode, null);
    }

    public void ShowList(FlashcardSelectionMode mode, string? message)
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
        view.SetMode(mode);
        view.SetMessage(message);
        view.Show();
    }

    public void SelectCard(FlashcardSelectionMode mode, long cardId)
    {
        switch (mode)
        {
            case FlashcardSelectionMode.ForEdit:
                ShowEdit(cardId);
                break;
            case FlashcardSelectionMode.ForDelete:
                ShowDelete(cardId);
                break;
        }
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

    public void ShowEdit(long cardId)
    {
        ShowEdit(cardId, null);
    }

    public void ShowEdit(long cardId, string? message)
    {
        var card = database.ReadFlashcardById(cardId);
        if (card == null)
        {
            ShowMenu("ERROR - Failed to read card from database.");
        }
        else
        {
            var view = new FlashcardEditView(this, card);
            view.SetMessage(message);
            view.Show();
        }
    }

    public void Update(long cardId, string? newFront, string? newBack)
    {
        if (String.IsNullOrEmpty(newFront) || String.IsNullOrWhiteSpace(newFront)
        || String.IsNullOrEmpty(newBack) || String.IsNullOrWhiteSpace(newBack))
        {
            ShowMenu();
            return;
        }

        var cleanFront = newFront.Trim();
        var cleanBack = newFront.Trim();
        if (database.UpdateFlashcard(cardId, cleanFront, cleanBack))
        {
            ShowMenu($"OK - Flashcard updated.");
        }
        else
        {
            ShowMenu("ERROR - Failed to update Flashcard.");
        }
    }

    public void ShowDelete(long cardId)
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