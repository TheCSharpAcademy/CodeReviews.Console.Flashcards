using Spectre.Console;
using Flashcards.DatabaseUtilities;

namespace Flashcards;
class Program
{
    public static string DatabasePath = System.Configuration.ConfigurationManager.AppSettings.Get("DatabasePath") ?? "";
    public static string ConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString") ?? "";
    static void Main(string[] args)
    {
        if (DatabasePath == "" || ConnectionString == "")
        {
            AnsiConsole.MarkupLine("[red]Database Path or Connection String not found. Please specify a path in the App configuration file.[/]");
            Environment.Exit(0);
        }

        DatabaseHelper.InitializeDatabase();
        DatabaseHelper.GetStacks();
        DatabaseHelper.GetSessions();
        MainMenu();
    }

    public static void MainMenu()
    {
        Console.Clear();
        AnsiConsole.WriteLine("Flashcards Main Menu\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(["Exit Program", "Study Sessions", "View Sessions", "Manage Stacks"]));

        switch (menu)
        {
            case "Exit Program":
                Environment.Exit(0);
                break;
            case "Study Sessions":
                Practice.PracticeMenu();
                break;
            case "View Sessions":
                Output.ViewSessions(true);
                break;
            case "Manage Stacks":
                Output.ViewStacks(true);
                break;
        }

        MainMenu();
    } // end of MainMenu Method
}