using Spectre.Console;

namespace FlashcardsLibrary;

public class UserInput
{
    public static string ShowMenu()
    {
        Console.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please choose an option from the menu:")
            .AddChoices(new[] {"Manage stacks", "Manage flashcards", "Study", "Show study history", "Generate report", "Exit"}));        
    }

    public static bool SwitchMenuChoice(string menuChoice)
    {
        try
        {
            switch (menuChoice)
            {
                case "Manage stacks":
                    SwitchStacksMenuChoice(ShowStacksMenu()); break;
                case "Manage flashcards":
                    SwitchFlashcardsMenuChoice(Validator.GetStackIdFromName(Validator.ValidateStackName()), ShowFlashcardsMenu()); break;
                case "Study":
                    Study.Run.Start(Validator.ValidateStackName()); break;
                case "Show study history":
                    PresentationLayer.ShowCurrentStudySessions(); break;
                case "Generate report":
                    SwitchReportsMenuChoice(ShowReportsMenu()); break;
                case "Exit":
                    return true;
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
            Console.ReadLine();
        }
        return false;
    }

    public static string ShowStacksMenu()
    {           
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please choose an option from the menu:")
            .AddChoices(new[] { "View all stacks", "Add a stack", "Delete a stack", "Update stack name", "Back to main menu" }));
    }

    public static void SwitchStacksMenuChoice(string stacksMenuChoice)
    {
        try
        {
            switch (stacksMenuChoice)
            {
                case "View all stacks":
                    PresentationLayer.ShowCurrentStacks();break;
                case "Add a stack":
                    DatabaseQueries.Run.InsertStacks(Validator.ValidateString(Validator.AskForNewName("stack name")));break;
                case "Delete a stack":
                    DatabaseQueries.Run.DeleteStacks(Validator.ValidateStackName());break;
                case "Update stack name":
                    DatabaseQueries.Run.UpdateStacks(Validator.ValidateStackName(), Validator.ValidateString(Validator.AskForNewName("stack name")));break;
                case "Back to main menu":return;
            }
            AnsiConsole.Markup("[green]Operation successful.[/]");
            Console.ReadLine();
            Console.Clear();
            SwitchStacksMenuChoice(ShowStacksMenu());
        }    
        catch (Exception e)
        {
            AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
            Console.ReadLine();
        }
    }

    public static string ShowFlashcardsMenu()
    {
        return AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Please choose an option from the menu")
            .AddChoices(new[] { "View all flashcards in current stack", "Create a flashcard in current stack", "Delete a flashcard", "Edit a flashcard", "Back to main menu" }));
    }

    public static void SwitchFlashcardsMenuChoice(int stackId, string flashcardsMenuChoice)
    {
        try
        {
            switch (flashcardsMenuChoice)
            {
                case "View all flashcards in current stack":
                    PresentationLayer.ShowCurrentFlashcards(stackId); break;
                case "Create a flashcard in current stack":
                    DatabaseQueries.Run.InsertFlashcards(stackId, Validator.ValidateString(Validator.AskForNewName("front")), Validator.ValidateString(Validator.AskForNewName("back"))); break; 
                case "Delete a flashcard":
                    DatabaseQueries.Run.DeleteFlashcards(Validator.ValidateFlashcardChoice(stackId)); break;
                case "Edit a flashcard":
                    DatabaseQueries.Run.UpdateFlashcards(Validator.ValidateFlashcardChoice(stackId), 
                        Validator.ValidateString(Validator.AskForNewName("front")), 
                        Validator.ValidateString(Validator.AskForNewName("back"))); break;                   
                case "Back to main menu": return;
            }
            AnsiConsole.Markup("[green]Operation successful.[/]");
            Console.ReadLine();
            Console.Clear();
            SwitchFlashcardsMenuChoice(stackId, ShowFlashcardsMenu());
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
            Console.ReadLine();
        }        
    }

    private static string ShowReportsMenu()
    {
        return AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Choose a report to generate:")
            .AddChoices("Number of sessions per month per stack", "Average score per month per stack", "Back to main menu"));
    }

    public static void SwitchReportsMenuChoice(string reportsMenuChoice)
    {
        try
        {
            switch (reportsMenuChoice)
            {
                case "Number of sessions per month per stack":
                    int year = Validator.ValidateYearChoiceForReport();
                    PresentationLayer.ShowReportCountSessionsPerMonth(DatabaseQueries.Run.SelectPivotSessionsPerMonth(year, "Count"), year, "Count"); break;
                case "Average score per month per stack":
                    year = Validator.ValidateYearChoiceForReport();
                    PresentationLayer.ShowReportCountSessionsPerMonth(DatabaseQueries.Run.SelectPivotSessionsPerMonth(year, "Avg"), year, "Average"); break;
                case "Back to main menu":
                    return;
            }
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
            Console.ReadLine();
        }
    }
}

