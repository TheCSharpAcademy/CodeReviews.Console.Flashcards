
class FlashCardsMenuController : MenuController
{
    static StacksDatabaseManager stacksDatabaseManager = new();
    static FlashcardsDatabaseManager flashcardsDatabaseManager = new();
    static Stack currentStack = default;
    static List<FlashcardDTO> flashcards = [];

    // Prompts user for stack to edit flashcards in
    protected override async Task OnReady()
    {
        List<Stack> stackSet = await stacksDatabaseManager.GetLogs();
        currentStack = GetInput.Selection(stackSet);
        Console.Clear();
    }

    // Shows first 10 cards to user
    protected override async Task MainAsync()
    {
        flashcards = await GetCards();
        ViewCards(10);
    }

    protected override async Task<bool> HandleMenuAsync()
    {
        Enums.ManageFlashCardsMenuOptions userInput = DisplayMenu.FlashCardsMenu();
        switch (userInput)
        {
            case Enums.ManageFlashCardsMenuOptions.VIEWALLCARDS:
                ViewAllCards();
                break;
            case Enums.ManageFlashCardsMenuOptions.CREATECARD:
                await CreateCard();
                break;
            case Enums.ManageFlashCardsMenuOptions.EDITCARD:
                await EditCard();
                break;
            case Enums.ManageFlashCardsMenuOptions.DELETECARD:
                await DeleteCard();
                break;
            case Enums.ManageFlashCardsMenuOptions.BACK:
                return true;
        }
        return false;
    }
    
    private static async Task<List<FlashcardDTO>> GetCards()
    {
        List<FlashcardDTO> flashcards = [];
        List<Flashcard> flashCardSet = await flashcardsDatabaseManager.GetLogs(currentStack);

        foreach (var card in flashCardSet)
        {
            flashcards.Add(new FlashcardDTO(card));
        }

        return flashcards;
    }

    private static void ViewCards(int amount = 100)
    {
        DisplayData.Table(flashcards.Take(amount).ToList(), currentStack.Name);
    }

    private static void ViewAllCards()
    {
        ViewCards();
    }

    private static async Task CreateCard()
    {        
        GetInput.FlashcardSides(out string front, out string back);
        
        await flashcardsDatabaseManager.InsertLog(new Flashcard(
            currentStack.Id,
            flashcards.Count + 1,
            front,
            back
        ));
    }

    private static async Task EditCard()
    {
        FlashcardDTO flashcard = GetInput.Selection(flashcards);
        GetInput.FlashcardSides(out string front, out string back, flashcard.Front, flashcard.Back);

        await flashcardsDatabaseManager.UpdateLog(new Flashcard(
            currentStack.Id,
            flashcard.Id,
            front,
            back
        ));
    }

    private static async Task DeleteCard()
    {
        FlashcardDTO flashcard = GetInput.Selection(flashcards);

        await flashcardsDatabaseManager.DeleteLog(flashcard.Id);
        await flashcardsDatabaseManager.UpdateIds(flashcard.Id);
    }
}