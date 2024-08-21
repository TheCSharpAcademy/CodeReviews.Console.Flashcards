using System.ComponentModel.DataAnnotations;

namespace Flashcards.Eddyfadeev.Enums;

/// <summary>
/// Represents the main menu entries.
/// </summary>
internal enum MainMenuEntries
{
    [Display(Name = "Start Study Session")]
    StartStudySession,
    [Display(Name = "Study History")]
    StudyHistory,
    [Display(Name = "Manage Stacks")] 
    ManageStacks,
    [Display(Name = "Manage Flashcards")] 
    ManageFlashcards,
    [Display(Name = "Exit")] 
    Exit
}