using Spectre.Console;

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

        MainMenu();
    }

    public static void MainMenu()
    {
        Console.Clear();
        AnsiConsole.WriteLine("Flashcard Main Menu\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices([
                "Exit Flashcard", "Practice Stack", "Create Stack",
                "Edit Stack", "View Stacks"
                ]));

        switch (menu)
        {
            case "Exit Flashcard":
                Environment.Exit(0);
                break;
            case "Practice Stack":
                Output.PracticeStack();
                break;
            case "Create Stack":
                Output.CreateStack();
                break;
            case "Edit Stack":
                Output.EditStack();
                break;
            case "View Stacks":
                Output.ViewStacks();
                break;
        }

        MainMenu();
    } // end of MainMenu Method
}