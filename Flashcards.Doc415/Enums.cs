namespace FlashCards.Doc415;

internal class Enums
{
    public enum MainMenuSelections
    {
        ManageStacks,
        ManageFlashcards,
        StudyArea,
        Quit
    }

    public enum StackSelections
    {
        ViewStacks,
        AddStack,
        RemoveStack,
        UpdateStack,
        DeleteStack,
        ReturnToMainMenu
    }

    public enum FlashcardSelections
    {
        ViewFlashcards,
        AddFlashcard,
        UpdateFlashcard,
        DeleteFlashcard,
        ReturnToMainMenu
    }

    public enum StudyAreaSelections
    {
        StartNewStudy,
        ViewStudies,
        Statistics,
        QuitToMainMenu
    }
}
