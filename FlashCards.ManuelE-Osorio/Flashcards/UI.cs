namespace Flashcards;

class UI
{
    public static void WelcomeMessage()
    {
        Helpers.ClearConsole();
        Console.WriteLine("Welcome to the Flash Cards App!\n");
        Thread.Sleep(2000);
    }
    public static void MainMenu(string? stackName, string? errorMessage)
    {
        Helpers.ClearConsole();
        Console.WriteLine($"Currently you have selected the stack \"{stackName}\"\n"); //pending if null
        if (errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine(
        "1) Manage stacks\n"+
        "2) Manage Flashcards\n"+
        "3) Start a study session\n"+
        "4) View study sessions data\n"+
        "0) Exit the application\n");
    }

    public static void Stacks(string? errorMessage)
    {
        Helpers.ClearConsole();
        if (errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }
        
        Console.WriteLine(
        "1) Create a new stack\n"+
        "2) Select a stack\n"+
        "3) Modify a stack\n"+
        "4) Delete a stack\n"+
        "0) Return to the main menu\n");
    }

    public static void FlashCards(string? stackName, string? errorMessage)
    {
        Helpers.ClearConsole();
        Console.WriteLine($"Currently you have selected the stack \"{stackName}\"\n");
        if (errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine(
        "1) View the cards in the current stack\n"+
        "2) Create a new card in the current stack\n"+
        "3) Edit a card in the current stack\n"+
        "4) Delete a card in the current stack\n"+
        "0) Return to the main menu\n"  
        );

    }

    public static void StudySession(string? errorMessage)
    {
        Helpers.ClearConsole();
        if (errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage+"\n");
        }

        Console.WriteLine(
        "1) Start study session\n"+
        "2) Modify parameters\n"
        );

    }

    public static void StudySessionData()
    {

    }

    public static void ExitMessage()
    {
        Helpers.ClearConsole();
        Console.WriteLine("Thank you for using the Flash Cards App!\n");
    }

    public static void NewStackName(string? errorMessage, string? action)
    {
        Helpers.ClearConsole();
        if(errorMessage != null) 
        {
            Console.WriteLine("Error: " + errorMessage);
        }

        Console.WriteLine($"Please write the {action} name of the stack. Valid characters are a to z, A to Z and 0 to 9.\n"+
        "The maximum length of the name is 50 characters.\n");
    }

    public static void SelectOrDeleteStack(List<StacksDTO> currentStacksUI, string? errorMessage, string action)
    {
        Helpers.ClearConsole();
        TableUI.PrintStacksTable(currentStacksUI);
        if(errorMessage != null) 
        {
            Console.WriteLine("Error: " + errorMessage);
        }

        Console.WriteLine($"Please write the name of the stack that you want to {action}:\n");
    }

    public static void DisplayCards(List<CardsDTO> currentCardsUI, Stacks? currentStack, string? errorMessage, string? action)
    {
        Helpers.ClearConsole();
        TableUI.PrintCardsTable(currentCardsUI, currentStack);
        if(errorMessage != null)
        {
            Console.WriteLine("Error: " + errorMessage);    
        }

        if(action != null)
        {
            Console.WriteLine($"Please write the ID of the card that you want to {action}:\n");
        }
        else
        {
            Console.WriteLine("Press any key to return");
        }
    }

    public static void NewCard(string? errorMessage, string? type, string? action)
    {
        Helpers.ClearConsole();
        if(errorMessage != null) 
        {
            Console.WriteLine("Error: " + errorMessage);
        }

        Console.WriteLine($"Please write the {action} {type} of the card. Valid characters are a to z, A to Z and 0 to 9.\n"+
        $"The maximum length of the {type} is 50 characters.\n");
    }

    public static void StudySessionQuestion(Cards card)
    {
        Helpers.ClearConsole();
        Console.WriteLine("What is the answer to the following question:\n"+
        $"{card.Question}");
    }

    public static void StudySessionAnswer(Cards card, bool answerIsCorrect)
    {
        Helpers.ClearConsole();
        Console.WriteLine("What is the answer to the following question:\n"+
        $"{card.Question}");

        if(answerIsCorrect)
        {
            Console.WriteLine("That is the correct answer. Press any key to continue");
        }
        else
        {
            Console.WriteLine($"Wrong answer. The correct answer is: {card.Answer}. Press any key to continue");
        }
    }
}