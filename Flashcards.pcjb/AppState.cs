namespace Flashcards;

static class AppState
{
    public enum Mode
    {
        ManageStacks,
        EditStack,
        DeleteStack,
        ManageFlashcards
    }

    public static Mode? CurrentMode { get; set; }
    public static Stack? ActiveStack { get; set; }
}