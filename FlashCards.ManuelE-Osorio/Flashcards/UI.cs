namespace Flashcards;

class UI
{
    public static void WelcomeMessage()
    {
        Helpers.ClearConsole();
        Console.WriteLine("Welcome to the Flash Cards App!\n");
        Thread.Sleep(2000);
    }
    public static void MainMenu()
    {
        Helpers.ClearConsole();
        Console.WriteLine(
        "1) Manage stacks\n"+
        "2) Manage Flashcards\n"+
        "3) Start a study session\n"+
        "4) View study sessions data\n");
    }

    public static void Stacks()
    {
        Helpers.ClearConsole();
        Console.WriteLine(
        "1) Create a new stack\n"+
        "2) Select a stack\n"+
        "3) Delete a stack\n");
    }

    public static void FlashCards()
    {

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

    public static void NewStackName(string? errorMessage)
    {
        Helpers.ClearConsole();
        if(errorMessage != null) Console.WriteLine("Error: " + errorMessage);

        Console.WriteLine("Please write the name of the stack. Valid characters are a to z, A to Z and 0 to 9.\n"+
        "The maximum length of the name is 50 characters.\n");
    }

    public static void SelectStack(string? errorMessage)
    {
        if(errorMessage != null) Console.Write("Error: " + errorMessage);
        Console.Write("Please write the name of the stack that you want to select:\n");
    }
}