using System.ComponentModel.DataAnnotations;

namespace Flashcards.RyanW84;

internal class Enums
{
    internal enum MainMenuChoices
    {
        [Display(Name = "Manage Stacks")]
        ManageStacks,

        [Display(Name = "Manage Flashcards")]
        ManageFlashcards,

        [Display(Name = "Study Session")]
        StudySession,

        [Display(Name = "Study History")]
        StudyHistory,

        [Display(Name = "Report: Monthly Session Count")]
        Reports,

        [Display(Name = "Quit")]
        Quit,
    }

    internal enum StackChoices
    {
        [Display(Name = "View Stacks")]
        ViewStacks,

        [Display(Name = "Add Stack")]
        AddStack,

        [Display(Name = "Delete Stack")]
        DeleteStack,

        [Display(Name = "Update Stack")]
        UpdateStack,

        [Display(Name = "Return to Main Menu")]
        ReturnToMainMenu,
    }

    internal enum FlashcardChoices
    {
        [Display(Name = "View Flashcards")]
        ViewFlashcards,

        [Display(Name = "Add Flashcard")]
        AddFlashcard,

        [Display(Name = "Delete Flashcard")]
        DeleteFlashcard,

        [Display(Name = "Update Flashcard")]
        UpdateFlashcard,

        [Display(Name = "Return to Main Menu")]
        ReturnToMainMenu,
    }
}
