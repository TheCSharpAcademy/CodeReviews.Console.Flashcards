using System.Threading.Tasks;
using Spectre.Console;

class FlashCardsMenuController : MenuController
{
    static Stack currentStack = default;
    static List<FlashcardDTO> flashcards = [];

    // Prompts user for stack to edit flashcards in
    protected override async Task OnReady()
    {
        List<Stack> stackSet = await DataBaseManager<Stack>.GetLogs();
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
        string query = "WHERE Stacks_Id = " + currentStack.Id;
        List<Flashcard> flashCardSet = await DataBaseManager<Flashcard>.GetLogs(query);

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
        
        await DataBaseManager<Flashcard>.InsertLog([
            currentStack.Id.ToString(),
            (flashcards.Count + 1).ToString(),
            "'" + front + "'",
            "'" + back + "'"
        ]);
    }

    private static async Task EditCard()
    {
        FlashcardDTO flashcard = GetInput.Selection(flashcards);
        GetInput.FlashcardSides(out string front, out string back, flashcard.Front, flashcard.Back);

        await DataBaseManager<Flashcard>.UpdateLog(
            "Id = " + flashcard.Id.ToString(),
            [
                "Front = '" + front + "'",
                "Back = '" + back + "'",
            ]
        );
    }

    private static async Task DeleteCard()
    {
        FlashcardDTO flashcard = GetInput.Selection(flashcards);

        await DataBaseManager<Flashcard>.DeleteLog(flashcard.Id);
        await DataBaseManager<Flashcard>.UpdateLog(
            "Id in (SELECT Id FROM flash_cards WHERE Id > "+ flashcard.Id.ToString() + ")",
            ["Id = Id - 1"]
        );
    }
}