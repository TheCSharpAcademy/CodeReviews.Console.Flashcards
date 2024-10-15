using System.Data.Common;
using System.Runtime.CompilerServices;
using Spectre.Console;
public static class Menu
{
    private static string userInput = "";
    private static string mainMenuChoice = "";
    public static string selectedStack = "No stack selected";
    public static void DisplayMenu()
    {

        while (mainMenuChoice != "Exit")
        {
            Console.Clear();
            // Create main prompt
            mainMenuChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select an option")
            .PageSize(10)
            .MoreChoicesText("(Move up and down to reveal more options)")
            .AddChoices(new[] {
                "Exit",
                "Manage stacks",
                "Manage flashcards",
                "Study",
                "View study session data"
            }));

            // Switch statement for menu options
            switch (mainMenuChoice.ToLower())
            {
                case "manage stacks":

                    selectedStack = StudyContentController.SelectStack();

                    break;

                case "manage flashcards":
                    while (mainMenuChoice != "Return to main menu")
                    {

                        mainMenuChoice = StudyContentController.ManageFlashcards();

                        // Add nested switch statement for submenu
                        switch (mainMenuChoice)
                        {
                            case "View all flashcards in stack":
                                Console.Clear();
                                List<Flashcard> flashcards = StudyContentController.GetFlashcardsInStack();

                                // Display table with ID, front, and back
                                Table table = new Table();

                                table.AddColumn("Id");
                                table.AddColumn("Front");
                                table.AddColumn("Back");

                                int id = 1;

                                foreach (Flashcard flashcard in flashcards)
                                {
                                    table.AddRow(id.ToString(), flashcard.front, flashcard.back);
                                    id++;
                                }

                                AnsiConsole.Write(table);

                                break;

                            case "Create a flashcard in current stack":
                                StudyContentController.CreateFlashcard();
                                break;

                            default:
                                break;
                        }
                    }

                    break;
            }
        }
    }
}