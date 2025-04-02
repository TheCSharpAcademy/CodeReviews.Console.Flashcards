using System.Threading.Tasks;
using Spectre.Console;

class ManageFlashCardsMenuController
{
    static Stack currentStack = default;
    static List<FlashcardDTO> flashcards = [];
    public static async Task Start()
    {
        bool exit = false;
        List<Stack> stackSet = await DataBaseManager<Stack>.GetLogs();
        Console.Clear();
        currentStack = GetInput.Selection(stackSet);
        while (!exit)
        {
            Console.Clear();
            flashcards = await GetCards();

            exit = await HandleUserInput();

            if (!exit)
            {
                AnsiConsole.Markup("[bold green]Press Enter to continue. [/]");
                Console.Read();
            }

        }
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

    private static async Task ViewCards(int amount = -1)
    {
        string query = "WHERE Stacks_Id = " + currentStack.Id;
        if (amount != -1)
            query += "ORDER BY Id OFFSET 0 ROWS FETCH FIRST " + amount + " ROWS ONLY";

        List<Flashcard> flashCardSet = await DataBaseManager<Flashcard>.GetLogs(query);
        List<FlashcardDTO> flashcardDTOs = [];

        foreach (var card in flashCardSet)
        {
            flashcardDTOs.Add(new FlashcardDTO(card));
        }
        DisplayData.Table(flashcardDTOs, currentStack.Name);
    }

    private static async Task ViewAllCards()
    {
        await ViewCards();
    }

    private static async Task ViewXCards()
    {
        await ViewCards(GetInput.AmountOfCards());
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


    private static async Task<bool> HandleUserInput()
    {
        Enums.ManageFlashCardsMenuOptions userInput = DisplayMenu.ManageFlashCardsMenu();
        switch (userInput)
        {
            case Enums.ManageFlashCardsMenuOptions.VIEWALLCARDS:
                await ViewAllCards();
                break;
            case Enums.ManageFlashCardsMenuOptions.VIEWXCARDS:
                await ViewXCards();
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
}