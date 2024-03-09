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
                        "Exit"
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
                    DatabaseController.CreateFlashCard();
                    break;
                case "Delete a Flash Card":
                    DatabaseController.DeleteFlashCard();
                    break;
                case "Study Session":
                    StudyFlashCards.NewStudySession();
                    break;
                case "View Study Sessions":
                    StudyFlashCards.GetStudySessions();
                    break;
                case "Exit":
                    appRunning = false;
                    Environment.Exit(0);
                    break;
            }
        } while (appRunning);
    }

}