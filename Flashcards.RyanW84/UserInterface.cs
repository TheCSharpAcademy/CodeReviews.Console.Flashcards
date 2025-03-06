using Spectre.Console;

namespace Flashcards.RyanW84;

internal class UserInterface
{
    public static string? usersChoice;
    public static bool isMenuRunning;

    internal static void MainMenu()
    {
        isMenuRunning = true;
        while (isMenuRunning)
        {
            isMenuRunning = true;
            Console.Clear();
            var usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices("Study Area", "Manage the Stacks", "Manage the Flashcards", "Quit")
            );
            MenuSelector(usersChoice);
        }
    }

    internal static void StackMenu()
    {
        isMenuRunning = true;
        while (isMenuRunning)
        {
            Console.Clear();
            usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        "Add a Stack",
                        "Delete a Stack",
                        "Update a Stack",
                        "View the Stacks",
                        "Exit to Main Menu"
                    )
            );
            MenuSelector(usersChoice);
        }
    }

    internal static void FlashcardMenu()
    {
        isMenuRunning = true;
        while (isMenuRunning)
        {
            Console.Clear();
            usersChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices(
                        "Add a Flashcard",
                        "Delete a Flashcard",
                        "Update a Flashcard",
                        "View the Flashcards",
                        "Exit to Main Menu"
                    )
            );
            MenuSelector(usersChoice);
        }
    }

    internal static void MenuSelector(string usersChoice)
    {
        var DataAccess = new DataAccess();
        switch (usersChoice)
        {
            case "Study Area":
                DataAccess.StudyArea();
                break;
            case "Manage the Stacks":
                StackMenu();
                break;
            case "Manage the Flashcards":
                FlashcardMenu();
                break;
            case "Add a Stack":
                isMenuRunning = false;
                DataAccess.AddStack();
                break;
            case "Delete a Stack":
                DataAccess.DeleteStack();
                break;
            case "Update a Stack":
                DataAccess.UpdateStack();
                break;
            case "View the Stacks":
                isMenuRunning = false;
                DataAccess = new DataAccess();
                var gotStacks = DataAccess.GetStacks();
                DataAccess.ViewStacks(gotStacks);
                break;
            case "Add a Flashcard":
                DataAccess.AddFlashcard();
                break;
            case "Delete a Flashcard":
                DataAccess.DeleteFlashcard();
                break;
            case "Update a Flashcard":
                DataAccess.UpdateFlashcard();
                break;
            case "View the Flashcards":
                DataAccess.GetFlashcards();
                break;
            case "Exit to Main Menu":
                MainMenu();
                break;
            case "Quit":
                System.Console.WriteLine("Goodbye");
                isMenuRunning = false;
                break;
        }
    }
}
