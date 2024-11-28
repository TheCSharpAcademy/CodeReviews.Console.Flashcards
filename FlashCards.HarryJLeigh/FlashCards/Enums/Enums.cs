using System.ComponentModel.DataAnnotations;

namespace FlashCards.Enums;

public enum MenuChoice
{
    [Display(Name = "exit")] exit,
    [Display(Name = "Manage Stacks")] ManageStacks,
    [Display(Name = "Manage FlashCards")] ManageFlashCards,
    [Display(Name = "Study")] Study,
    [Display(Name = "View study sessions data")]
    ViewStudySessionData
}

public enum FlashcardViewMenu
{
    [Display(Name = "Return to previous menu")] ReturnToMenu,
    [Display(Name = "Change current stack")] ChangeStack,
    [Display(Name = "View all Flashcards in a stack")] ViewAll,
    [Display(Name = "View X amount of Flashcards in a stack")] ViewAmountOfFlashCards,
    [Display(Name = "Create a FlashCard in a current stack")] CreateFlashcard,
    [Display(Name = "Edit a Flashcard")] EditFlashcard,
    [Display(Name = "Delete a Flashcard")] DeleteFlashcard,
}

internal enum StackViewMenu
{
    [Display(Name = "Return to previous menu")] ReturnToMenu,
    [Display(Name = "Create a stack")] CreateStack,
    [Display(Name = "View stacks")] ViewStacks,
    [Display(Name = "Update stack name")] UpdateStack,
    [Display(Name = "Delete stack")] DeleteStack,
}

internal enum StudyViewMenu
{
    [Display(Name = "Return to previous menu")] ReturnToMenu,
    [Display(Name = "View all study sessions")] ViewAll,
    [Display(Name = "Start study session")] StartStudy,
}

internal enum ReportViewMenu
{
    [Display(Name = "Return to previous menu")] ReturnToMenu,
    [Display(Name = "Number of sessions per month ")] SessionsPerMonth,
    [Display(Name = "Average score per month")] AverageScorePerMonth,
}
