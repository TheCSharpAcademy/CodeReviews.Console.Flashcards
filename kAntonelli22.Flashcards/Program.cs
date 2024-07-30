using Spectre.Console;

namespace Flashcards;
class Program
{
    public static string DatabasePath = System.Configuration.ConfigurationManager.AppSettings.Get("DatabasePath") ?? "";
    public static string ConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
    static void Main(string[] args)
    {
        Console.SetWindowSize(100, Console.WindowHeight);
        if (DatabasePath == "" || ConnectionString == "")
        {
            AnsiConsole.MarkupLine("[red]Database Path or Connection String not found. Please specify a path in the App configuration file.[/]");
            Environment.Exit(0);
        }

        CardStack stack1 = new CardStack("Stack 1", 5);
        for (int i = 0; i < 5; i++)
            new Card($"question {i}", $"answer {i}", stack1);
        CardStack stack2 = new CardStack("Stack 2", 8);
        for (int i = 0; i < 8; i++)
            new Card($"question {i}", $"answer {i}", stack2);
        CardStack stack3 = new CardStack("Stack 3", 2);
        for (int i = 0; i < 2; i++)
            new Card($"question {i}", $"answer {i}", stack3);

        MainMenu();
    }

    public static void MainMenu()
    {
        Console.Clear();
        AnsiConsole.WriteLine("Flashcard Main Menu\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(["Exit Flashcard", "Practice Stack", "View Stacks"]));

        switch (menu)
        {
            case "Exit Flashcard":
                Environment.Exit(0);
                break;
            case "Practice Stack":
                Practice.PracticeStack();
                break;
            case "View Stacks":
                Output.ViewStacks(true);
                break;
        }

        MainMenu();
    } // end of MainMenu Method
}