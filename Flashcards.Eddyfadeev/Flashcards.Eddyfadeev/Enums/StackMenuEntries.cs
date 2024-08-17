using System.ComponentModel.DataAnnotations;

namespace Flashcards.Eddyfadeev.Enums;

/// <summary>
/// Represents the stack menu entries.
/// </summary>
internal enum StackMenuEntries
{
    [Display(Name = "View Stacks")] 
    ChooseStack,
    [Display(Name = "Add Stack")] 
    AddStack,
    [Display(Name = "Delete Stack")] 
    DeleteStack,
    [Display(Name = "Edit Stack")] 
    EditStack,
    [Display(Name = "Return to Main Menu")]
    ReturnToMainMenu
}