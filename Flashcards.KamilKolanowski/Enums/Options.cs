namespace Flashcards.KamilKolanowski.Enums;

internal class Options
{
    internal enum MenuOptions
    {
        MFlashcards,
        MStacks,
        StudySession,
        ViewStudySessions,
        Exit
    };

    internal static Dictionary<MenuOptions, string> MenuOptionsDisplay { get; } = new Dictionary<MenuOptions, string>()
    {
        [MenuOptions.MFlashcards] = "Manage Flashcards",
        [MenuOptions.MStacks] = "Manage Stacks",
        [MenuOptions.StudySession] = "Start Study Sessions",
        [MenuOptions.ViewStudySessions] = "View Study Sessions",
        [MenuOptions.Exit] = "Exit"
    };

    internal enum DBOptions
    {
        AddRow,
        UpdateRow,
        DeleteRow,
        ViewRows
    }

    internal static Dictionary<DBOptions, string> FlashcardsOptionDisplay { get; } = new Dictionary<DBOptions, string>()
    {
        [DBOptions.AddRow] = "Add new Flashcard",
        [DBOptions.UpdateRow] = "Edit Flashcard",
        [DBOptions.DeleteRow] = "Delete Flashcard",
        [DBOptions.ViewRows] = "View Flashcard"
    };
    
    internal static Dictionary<DBOptions, string> StacksOptionDisplay { get; } = new Dictionary<DBOptions, string>()
    {
        [DBOptions.AddRow] = "Add new Stack",
        [DBOptions.UpdateRow] = "Edit Stack",
        [DBOptions.DeleteRow] = "Delete Stack",
        [DBOptions.ViewRows] = "View Stack"
    };
}