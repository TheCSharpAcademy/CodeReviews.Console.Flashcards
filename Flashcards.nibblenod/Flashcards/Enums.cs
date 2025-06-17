using System.ComponentModel.DataAnnotations;
namespace Flashcards
{
    internal class Enums
    {
        internal enum MainMenuOptions
        {
            Exit,

            [Display(Name = "Manage Stacks")]
            Manage_Stacks,

            [Display(Name = "Manage Flashcards")]
            Flashcards,

            Study,

            [Display(Name = "View Study Session Data")]
            View_Study_Session_Data,
        }
        internal enum StackManagementOptions
        {
            Add,
            Remove,
            Edit,
            View,
            Exit
        }
        internal enum FlashcardManagementOptions
        {
            [Display(Name = "Main Menu")]
            Main_Menu,

            [Display(Name = "Change Current Stack")]
            Current_Stack,

            [Display(Name = "View Flashcards")]
            View_Flashcards,

            [Display(Name = "View X Number of Cards")]
            View_X_Number_Of_Cards,

            [Display(Name = "Create Flashcard")]
            Create_Flashcard,

            [Display(Name = "Edit Flashcard")]
            Edit_Flashcard,

            [Display(Name = "Delete Flashcard")]
            Delete_Flashcard
        }

        internal enum EditType
        {
            Front,
            Back,
        }

    }
}
