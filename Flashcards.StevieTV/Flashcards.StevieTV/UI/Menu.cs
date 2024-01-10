using Spectre.Console;

namespace Flashcards.StevieTV.UI;

public class Menu
{
    public static void MainMenu()
    {
        var exitApp = false;

        while (!exitApp)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("FlashCards App")
                .LeftJustified()
                .Color(Color.Red));
            //AnsiConsole.MarkupLine("Welcome to the [bold]FlashCards App[/]");
    
            var menuSelection = new SelectionPrompt<string>();
            menuSelection.Title("Please choose an option from the list below");
            menuSelection.AddChoice("Exit");
            menuSelection.AddChoice("Manage Stacks");
            menuSelection.AddChoice("Manage Flashcards");
            menuSelection.AddChoice("Begin a Study Session");
            menuSelection.AddChoice("View Study Sessions");

            var menuInput = AnsiConsole.Prompt(menuSelection);
          
            switch (menuInput)
            {
                case "Exit":
                    AnsiConsole.WriteLine("Goodbye");
                    exitApp = true;
                    Environment.Exit(0);
                    break;
                case "Manage Stacks":
                    ManageStacks.StacksMenu();
                    break;
                case "Manage Flashcards":
                    ManageStacks.EditStack();
                    break;
                case "Begin a Study Session":
                    StudyGame.ChooseStudyStack();
                    break;
                case "View Study Sessions":
                    StudySessionHistory.ShowStudySessions();
                    break;
            }
        }
    }


}