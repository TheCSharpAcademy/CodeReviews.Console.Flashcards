namespace Flashcards.KamilKolanowski.Enums;

internal class Options
{
    internal enum MenuOptions
    {
        MFlashcards,
        MStacks,
        StudySession,
        ViewStudySessions,
        Exit,
    };

    internal static Dictionary<MenuOptions, string> MenuOptionsDisplay { get; } =
        new()
        {
            [MenuOptions.MFlashcards] = "Manage Flashcards",
            [MenuOptions.MStacks] = "Manage Stacks",
            [MenuOptions.StudySession] = "Start Study Sessions",
            [MenuOptions.ViewStudySessions] = "View Study Sessions",
            [MenuOptions.Exit] = "Exit",
        };

    internal enum DBOptions
    {
        AddRow,
        UpdateRow,
        DeleteRow,
        ViewRows,
    }

    internal static Dictionary<DBOptions, string> FlashcardsOptionDisplay { get; } =
        new()
        {
            [DBOptions.AddRow] = "Add new Flashcard",
            [DBOptions.UpdateRow] = "Edit Flashcard",
            [DBOptions.DeleteRow] = "Delete Flashcard",
            [DBOptions.ViewRows] = "View Flashcard",
        };

    internal static Dictionary<DBOptions, string> StacksOptionDisplay { get; } =
        new()
        {
            [DBOptions.AddRow] = "Add new Stack",
            [DBOptions.UpdateRow] = "Edit Stack",
            [DBOptions.DeleteRow] = "Delete Stack",
            [DBOptions.ViewRows] = "View Stack",
        };

    internal enum ViewStudySessionOptions
    {
        ViewStudySessions,
        ViewStudySessionsAggregated,
        ViewStudySessionsAverageScore,
    }

    internal static Dictionary<ViewStudySessionOptions, string> ViewStudySessionsDisplay { get; } =
        new()
        {
            [ViewStudySessionOptions.ViewStudySessions] = "View Study Sessions",
            [ViewStudySessionOptions.ViewStudySessionsAggregated] =
                "View Study Sessions Aggregated",
            [ViewStudySessionOptions.ViewStudySessionsAverageScore] =
                "View Study Sessions Average Score",
        };
}
