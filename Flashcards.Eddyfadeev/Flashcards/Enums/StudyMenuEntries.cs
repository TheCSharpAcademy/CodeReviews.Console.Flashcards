using System.ComponentModel.DataAnnotations;

namespace Flashcards.Enums;

/// <summary>
/// Represents the study menu entries.
/// </summary>
internal enum StudyMenuEntries
{
    [Display(Name = "Start Study Session")]
    StartStudySession,
    [Display(Name = "Study History")]
    StudyHistory,
    [Display(Name = "Return to Main Menu")]
    ReturnToMainMenu
}