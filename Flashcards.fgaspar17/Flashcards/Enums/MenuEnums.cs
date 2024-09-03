using CodingTracker;

namespace Flashcards.MenuEnums;

public enum MainMenuOptions
{
    [Title("Quit")]
    Quit,
    [Title("Manage Stacks")]
    ManageStacks,
    [Title("Manage Flashcards")]
    ManageFlashcards,
    [Title("Study")]
    Study,
    [Title("Show Study Sessions")]
    StudySessions,
    [Title("Monthly Report")]
    MonthlyReport,
}

public enum StackMenuOptions
{
    [Title("Back")]
    Back,
    [Title("Create Stack")]
    CreateStack,
    [Title("Update Stack")]
    UpdateStack,
    [Title("Delete Stack")]
    DeleteStack,
    [Title("Show Stacks")]
    ShowStacks,
}

public enum FlashcardMenuOptions
{
    [Title("Back")]
    Back,
    [Title("Change current Stack")]
    ChangeStack,
    [Title("View all Flashcards in the Stack")]
    ViewAll,
    [Title("View X amount of Flashcards in the Stack")]
    ViewX,
    [Title("Create Flashcard in the Stack")]
    CreateFlashcard,
    [Title("Update Flashcard in the Stack")]
    UpdateFlashcard,
    [Title("Delete Flashcard in the Stack")]
    DeleteFlashcard,
}

public enum MonthlyReportOptions
{
    [Title("Back")]
    Back,
    [Title("Show Sessions per Month and Stack")]
    SessionsPerMonth,
    [Title("Show Average Score per Month and Stack")]
    AveragePerMonth,
}