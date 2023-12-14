namespace Flashcards;

class UI
{
    public static void WelcomeMessage()
    {
        Helpers.ClearConsole();
        Console.WriteLine("Welcome to the Flash Cards App!\n");
        Thread.Sleep(2000);
    }
    public static void MainMenu(string? stackName)
    {
        Helpers.ClearConsole();
        Console.WriteLine($"Currently you have selected the stack \"{stackName}\"\n");

        Console.WriteLine(
        "1) Manage stacks\n"+
        "2) Manage Flashcards\n"+
        "3) Start a study session\n"+
        "4) View study sessions data\n"+
        "0) Exit the application\n");
    }

    public static void Stacks()
    {
        Helpers.ClearConsole();
        Console.WriteLine(
        "1) Create a new stack\n"+
        "2) Select a stack\n"+
        "3) Modify a stack\n"+
        "4) Delete a stack\n"+
        "0) Return to the main menu\n");
    }

    public static void FlashCards(string? stackName)
    {
        Helpers.ClearConsole();
        Console.WriteLine($"Currently you have selected the stack \"{stackName}\"\n");

        Console.WriteLine(
        "1) View the cards in the current stack\n"+
        "2) Create a new card in the current stack\n"+
        "3) Edit a card in the current stack\n"+
        "4) Delete a cards in the current stack\n"+
        "0) Return to the main menu\n"  
        );

    }

    public static void StudySession()
    {

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
        TableUI.PrintTable(currentStacksUI);
        if(errorMessage != null) 
        {
            Console.WriteLine("Error: " + errorMessage);
        }

        Console.WriteLine($"Please write the name of the stack that you want to {action}:\n");
    }
}