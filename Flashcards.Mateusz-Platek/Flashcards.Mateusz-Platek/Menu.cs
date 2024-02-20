using Flashcards.Mateusz_Platek.Controllers;
using Flashcards.Mateusz_Platek.Models;
using Spectre.Console;

namespace Flashcards.Mateusz_Platek;

public static class Menu
{
    public static void ContinueInput()
    {
        AnsiConsole.Prompt(
            new TextPrompt<string>("[bold yellow]Press enter to continue[/]")
                .AllowEmpty()
        );
    }

    public static string GetDummyData()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold underline red]Do you want to insert dummy data:[/]")
                .PageSize(3)
                .AddChoices(["Yes", "No"])
        );
    }
    
    private static string GetMenuOption()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold underline red]Menu[/]")
                .PageSize(6)
                .AddChoices(["Exit", "Manage stacks", "Study"])
        );
    }
    
    public static void Run()
    {
        bool end = false;
        while (!end)
        {
            string option = GetMenuOption();
            switch (option)
            {
                case "Exit":
                    end = true;
                    break;
                case "Manage stacks":
                    RunStacks();
                    break;
                case "Study":
                    RunStudy();
                    break;
            }
        }
    }

    private static string GetStudyOption()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold underline red]Study[/]")
                .PageSize(6)
                .AddChoices(["Move back", "View sessions", "Start new session", "View taken sessions", "View scores"])
        );
    }
    
    private static void RunStudy()
    {
        bool end = false;
        while (!end)
        {
            string option = GetStudyOption();
            switch (option)
            {
                case "Move back":
                    end = true;
                    break;
                case "View sessions":
                    SessionController.DisplaySessions();
                    ContinueInput();
                    break;
                case "Start new session":
                    SessionController.AddSession();
                    ContinueInput();
                    break;
                case "View taken sessions":
                    SessionController.DisplaySessionsReport();
                    ContinueInput();
                    break;
                case "View scores":
                    SessionController.DisplayAverageScore();
                    ContinueInput();
                    break;
            }
        }
    }

    private static string GetStacksOption()
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[bold underline red]Stacks[/]")
                .PageSize(6)
                .AddChoices(["Move back", "View stacks", "Add stack", "Manage stack", "Update stack", "Remove stack"])
        );
    }
    
    private static void RunStacks()
    {
        bool end = false;
        while (!end)
        {
            string option = GetStacksOption();
            switch (option)
            {
                case "Move back":
                    end = true;
                    break;
                case "View stacks":
                    StackController.DisplayStacks();
                    ContinueInput();
                    break;
                case "Add stack":
                    StackController.AddStack();
                    ContinueInput();
                    break;
                case "Update stack":
                    StackController.UpdateStack();
                    ContinueInput();
                    break;
                case "Manage stack":
                    Stack? stack = StackController.SelectStack();
                    if (stack == null)
                    {
                        break;
                    }
                    RunStack(stack.name);
                    break;
                case "Remove stack":
                    StackController.DeleteStack();
                    ContinueInput();
                    break;
            }
        }
    }

    private static string GetStackOption(string stackName)
    {
        AnsiConsole.Clear();
        return AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[bold underline red]{stackName}[/]")
                .PageSize(6)
                .AddChoices(["Move back", "View flashcards", "Add flashcard", "Update flashcard", "Remove flashcard"])
        );
    }
    
    private static void RunStack(string stackName)
    {
        bool end = false;
        while (!end)
        {
            string option = GetStackOption(stackName);
            switch (option)
            {
                case "Move back":
                    end = true;
                    break;
                case "View flashcards":
                    FlashcardController.DisplayFlashcards(stackName);
                    ContinueInput();
                    break;
                case "Add flashcard":
                    FlashcardController.AddFlashcard(stackName);
                    ContinueInput();
                    break;
                case "Update flashcard":
                    FlashcardController.UpdateFlashcard(stackName);
                    ContinueInput();
                    break;
                case "Remove flashcard":
                    FlashcardController.DeleteFlashcard(stackName);
                    ContinueInput();
                    break;
            }
        }
    }
}