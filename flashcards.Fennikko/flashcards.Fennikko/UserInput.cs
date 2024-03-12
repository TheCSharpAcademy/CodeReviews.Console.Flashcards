using Spectre.Console;

namespace flashcards.Fennikko;

public class UserInput
{
    public static void GetUserInput()
    {
        DatabaseController.DatabaseCreation();
        AnsiConsole.Clear();
        var appRunning = true;

        do
        {
            AnsiConsole.Clear();
            var functionSelect = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a [blue]function[/].")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Add a Stack", "Delete a Stack", "Add a Flash Card",
                        "Delete a Flash Card", "Study Session","View Study Sessions",
                        "Study Session Report","Exit"
                    }));
            switch (functionSelect)
            {
                case "Add a Stack":
                    DatabaseController.CreateStack();
                    break;
                case "Delete a Stack":
                    DatabaseController.DeleteStack();
                    break;
                case "Add a Flash Card":
                    DatabaseController.CreateFlashcard();
                    break;
                case "Delete a Flash Card":
                    DatabaseController.DeleteFlashcard();
                    break;
                case "Study Session":
                    StudyFlashcards.NewStudySession();
                    break;
                case "View Study Sessions":
                    StudyFlashcards.GetStudySessions();
                    break;
                case "Study Session Report":
                    StudyFlashcards.StudySessionReport();
                    break;
                case "Exit":
                    appRunning = false;
                    Environment.Exit(0);
                    break;
            }
        } while (appRunning);
    }

}