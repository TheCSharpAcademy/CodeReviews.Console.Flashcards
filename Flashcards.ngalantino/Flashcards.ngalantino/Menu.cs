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

                    if (selectedStack != null)
                    {
                        goto case "manage flashcards";
                    }

                    break;

                case "manage flashcards":

                    mainMenuChoice = StudyContentController.ManageFlashcards();

                    // Change current stack
                    if (mainMenuChoice == "Change current stack")
                    {
                        goto case "manage stacks";
                    }
                    else if (mainMenuChoice == "View all flashcards in stack")
                    {
                        goto case "View all flashcards in stack";
                    }

                    break;

                case "View all flashcards in stack":
                    List<Flashcard> flashcards = StudyContentController.GetFlashcardsInStack();

                    // Display table with ID, front, and back
                    Table table = new Table();

                    table.AddColumn("Id");
                    table.AddColumn("Front");
                    table.AddColumn("Back");

                    int id = 1;

                    foreach (Flashcard flashcard in flashcards) {
                        table.AddRow(id.ToString(), flashcard.front, flashcard.back);
                        id++;
                    }

                    AnsiConsole.Write(table);

                    break;
            }
        }
    }

}