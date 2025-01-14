using Spectre.Console;

internal class FlashcardsMenu
{
    private static readonly Dictionary<string, Action> _flashcardsMenuActions = new()
    {
        { DisplayInfoHelpers.Back, Console.Clear },
        { "View all flashcards in a stack", FlashcardRead.ShowAllFlashcards },
        { "Create flashcard", FlashcardCreate.AddFlashcards },
        { "Update flashcard", FlashcardUpdate.UpdateFlashcard },
        { "Delete flashcard", FlashcardDelete.DeleteFlashcard }
    };

    internal static void ShowFlashcardsMenu()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Choose an action with flashcard: ")
            .PageSize(10)
            .AddChoices(_flashcardsMenuActions.Keys));

        _flashcardsMenuActions[choice]();
    }
}
