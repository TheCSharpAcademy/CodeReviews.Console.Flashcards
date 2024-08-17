using System.ComponentModel.DataAnnotations;

namespace Flashcards.Eddyfadeev.Enums;

/// <summary>
/// Represents the flashcard menu entries.
/// </summary>
internal enum FlashcardEntries
{
    [Display(Name = "View Flashcards")] 
    ChooseFlashcard,
    [Display(Name = "Add Flashcard")] 
    AddFlashcard,
    [Display(Name = "Delete Flashcard")] 
    DeleteFlashcard,
    [Display(Name = "Edit Flashcard")] 
    EditFlashcard,
    [Display(Name = "Return to Main Menu")] 
    ReturnToMainMenu
}