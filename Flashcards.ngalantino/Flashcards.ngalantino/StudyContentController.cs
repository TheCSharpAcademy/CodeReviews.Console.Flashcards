using Spectre.Console;

public static class StudyContentController
{
    private static DatabaseManager db = new DatabaseManager();
    public static string SelectStack()
    {
        SelectionPrompt<string> prompt = new SelectionPrompt<string>();

        // Build the prompt
        foreach (Stack stack in db.GetStacks())
        {
            prompt.AddChoice(stack.name);
        }
        // Display the prompt
        string menuChoice = AnsiConsole.Prompt(prompt);

        return menuChoice;
    }

    public static string ManageFlashcards()
    {

        SelectionPrompt<string> prompt2 = new SelectionPrompt<string>();

        string mainMenuChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title($"Current working stack: {Menu.selectedStack}")
        .PageSize(10)
        .MoreChoicesText("(Move up and down to reveal more options)")
        .AddChoices(new[] {
                        "Return to main menu",
                        "Change current stack",
                        "View all flashcards in stack",
                        "View X amount of cards in stack",
                        "Create a flashcard in current stack",
                        "Edit a flashcard",
                        "Delete a flashcard"
        }));

        return mainMenuChoice;

    }

    public static List<Flashcard> GetFlashcardsInStack() {
        List<Flashcard> flashcards = db.GetFlashcards(Menu.selectedStack);

        return flashcards;
    }
}