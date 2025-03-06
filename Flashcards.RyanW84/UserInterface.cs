using Spectre.Console;

namespace Flashcards.RyanW84;

internal class UserInterface
{
    public static string? usersChoice;
    public static bool isMenuRunning = true;

    internal static void MainMenu()
    {
        while (isMenuRunning)
        {
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
                        "Add A Stacks",
                        "Delete a Stacks",
                        "Update a Stacks",
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
                        "Add A Flashcard",
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
            case "Add a Stacks":
                DataAccess.AddStack();
                break;
            case "Delete a Stacks":
                DataAccess.DeleteStack();
                break;
            case "Update a Stacks":
                DataAccess.UpdateStack();
                break;
            case "View the Stacks":
                DataAccess.GetStacks();
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
