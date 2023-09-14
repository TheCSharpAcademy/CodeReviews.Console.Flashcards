namespace Flashcards;

static class AppState
{
    public enum Mode
    {
        ManageStacks,
        ManageFlashcards
    }

    public static Mode? CurrentMode { get; set; }
    public static Stack? CurrentWorkingStack { get; set; }
}