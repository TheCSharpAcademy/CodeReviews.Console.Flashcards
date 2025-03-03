using System.ComponentModel.DataAnnotations;

namespace Flashcards.RyanW84;

internal static class UserInterface { }

internal class enums
{
    internal enum MainMenuChoices
    {
        [Display(Name = "Study Area")]
        Study,

        [Display(Name = "Manage the Stacks")]
        ManageStacks,

        [Display(Name = "Manage the Flashcards")]
        ManageFlashCards,

        [Display(Name = "Quit")]
        Quit,
    }

    internal enum StackMenuChoices
    {
        [Display(Name = "Add a Stack")]
        AddStack,

        [Display(Name = "Delete a Stack")]
        DeleteStack,

        [Display(Name = "Update a Stack")]
        UpdateStack,

        [Display(Name = "View the Stacks")]
        ViewStacks,

        [Display(Name = "Exit to Main Menu")]
        MainMenu,
    }

    internal enum FlashcardMenuChoices
    {
        [Display(Name = "Add a Flashcard")]
        AddFlashcard,

        [Display(Name = "Delete a Flashcard")]
        DeleteFlashcard,

        [Display(Name = "Update a Flashcard")]
        UpdateFlashcard,

        [Display(Name = "View Flashcards")]
        ViewFlashcards,

        [Display(Name = "Exit to Main Menu")]
        MainMenu,
    }
}
