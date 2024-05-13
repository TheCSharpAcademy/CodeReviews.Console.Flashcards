namespace FlashardsUI;
internal class Enums
{
    public enum MainMenuOptions
    {
        Stacks,
        Flashcards,
        StudySessions,
        Exit
    }

    public enum StacksMenuOptions
    {
        ViewAllStacks,
        AddStack,
        DeleteStack,
        UpdateStack,
        MainMenu
    }

    public enum FlashcardsMenuOptions
    {
        ChangeStack,
        ViewAllFlashcards,
        AddFlashcard,
        DeleteFlashcard,
        UpdateFlashcard,
        MainMenu
    }

    public enum StudySessionMenuOptions
    {
        ViewAllSessions,
        StartStudySession,
        MainMenu
    }
}
