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

        DatabaseHelper.GetStacks();
        DatabaseHelper.GetSessions();
        MainMenu();
    }

    public static void MainMenu()
    {
        Console.Clear();
        AnsiConsole.WriteLine("Flashcard Main Menu\n------------------------");
        var menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .AddChoices(["Exit Flashcard", "Study Sessions", "View Sessions", "Manage Stacks"]));

        switch (menu)
        {
            case "Exit Flashcard":
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

// -- Todo List
// - add or remove cards when changing stack size
// - allow user to edit a stacks flashcard
// - create study session functionality
// - create study session table
// - create study session viewer
// - remove study sessions when its stack is deleted

// -- Project Requirments
// - After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
// - The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
// - The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.