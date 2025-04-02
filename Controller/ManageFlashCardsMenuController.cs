using System.Threading.Tasks;
using Spectre.Console;

class ManageFlashCardsMenuController
{
    static Stack currentStack = default;
    public static async Task Start()
    {
        bool exit = false;

        Console.Clear();
        List<Stack> stackSet = await DataBaseManager<Stack>.GetLogs();
        currentStack = GetInput.Selection(stackSet);
        while (!exit)
        {
            Console.Clear();

            exit = await HandleUserInput();

            if (!exit)
            {
                AnsiConsole.Markup("[bold green]Press Enter to continue. [/]");
                Console.Read();
            }

        }
    }

    private static async Task ViewCardsAsync(int amount = -1)
    {
        string query = "WHERE Stacks_Id = " + currentStack.Id;
        if (amount != -1)
            query += "ORDER BY Id DESC OFFSET 0 ROWS FETCH FIRST " + amount + " ROWS ONLY";

        List<Flashcard> flashCardSet = await DataBaseManager<Flashcard>.GetLogs(query);
        List<FlashcardDTO> flashcardDTOs = [];

        foreach (var card in flashCardSet)
        {
            flashcardDTOs.Add(new FlashcardDTO(card));
        }
        DisplayData.Table(flashcardDTOs, currentStack.Name);
    }

    private static async Task CreateCard()
    {
        throw new NotImplementedException();
        //await DataBaseManager<Flashcard>.InsertLog();
    }

    private static void EditCard()
    {
        throw new NotImplementedException();
    }

    private static void DeleteCard()
    {
        throw new NotImplementedException();
    }


    private static async Task<bool> HandleUserInput()
    {
        Enums.ManageFlashCardsMenuOptions userInput = DisplayMenu.ManageFlashCardsMenu();
        switch (userInput)
        {
            case Enums.ManageFlashCardsMenuOptions.VIEWALLCARDS:
                await ViewCardsAsync();
                break;
            case Enums.ManageFlashCardsMenuOptions.VIEWXCARDS:
                await ViewCardsAsync(10);
                break;
            case Enums.ManageFlashCardsMenuOptions.CREATECARD:
                await CreateCard();
                break;
            case Enums.ManageFlashCardsMenuOptions.EDITCARD:
                EditCard();
                break;
            case Enums.ManageFlashCardsMenuOptions.DELETECARD:
                DeleteCard();
                break;
            case Enums.ManageFlashCardsMenuOptions.BACK:
                return true;
        }

        return false;
    }
}