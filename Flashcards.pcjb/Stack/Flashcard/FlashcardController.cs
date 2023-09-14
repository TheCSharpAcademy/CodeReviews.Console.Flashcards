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

    public void ShowList(Stack stack)
    {
        ShowList(stack, null);
    }

    public void ShowList(Stack stack , string? message)
    {
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