using System.ComponentModel.DataAnnotations;

namespace Flashcards.Enums;

/// <summary>
/// Represents the main menu entries.
/// </summary>
internal enum MainMenuEntries
{
    [Display(Name = "Study Menu")] 
    StudyMenu,
    [Display(Name = "Manage Stacks")] 
    ManageStacks,
    [Display(Name = "Manage Flashcards")] 
    ManageFlashcards,
    [Display(Name = "Exit")] 
    Exit
}