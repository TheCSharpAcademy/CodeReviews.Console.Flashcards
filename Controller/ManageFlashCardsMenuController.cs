using Spectre.Console;

class ManageFlashCardsMenuController
{
    public static async Task Start()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            List<Stack> stackSet = await DataBaseManager<Stack>.GetAllLogs();
            Stack currentStack = GetInput.Selection(stackSet);
            List<Flashcard> flashCardSet = await DataBaseManager<Flashcard>.GetAllLogs();
            List<FlashcardDTO> flashcardDTOs = default;

            foreach (var card in flashCardSet)
            {
                flashcardDTOs.Add(new FlashcardDTO(card));
            }
            DisplayData.Table(flashcardDTOs, currentStack.Name);

            Enums.ManageFlashCardsMenuOptions userInput = DisplayMenu.ManageFlashCardsMenu();
            switch (userInput)
            {
                case Enums.ManageFlashCardsMenuOptions.VIEWALLCARDS:
                    ViewAllCards();
                    break;
                case Enums.ManageFlashCardsMenuOptions.VIEWXCARDS:
                    ViewXCards();
                    break;
                case Enums.ManageFlashCardsMenuOptions.CREATECARD:
                    CreateCard();
                    break;
                case Enums.ManageFlashCardsMenuOptions.EDITCARD:
                    EditCard();
                    break;
                case Enums.ManageFlashCardsMenuOptions.DELETECARD:
                    DeleteCard();
                    break;
                case Enums.ManageFlashCardsMenuOptions.BACK:
                    exit = true;
                    break;
            }

            if (!exit)
            {
                AnsiConsole.Markup("[bold green]Press Enter to continue. [/]");
                Console.Read();
            }

        }
    }

    private static void ViewAllCards()
    {
        throw new NotImplementedException();
    }

    private static void ViewXCards()
    {
        throw new NotImplementedException();
    }

    private static void CreateCard()
    {
        throw new NotImplementedException();
    }

    private static void EditCard()
    {
        throw new NotImplementedException();
    }

    private static void DeleteCard()
    {
        throw new NotImplementedException();
    }
}