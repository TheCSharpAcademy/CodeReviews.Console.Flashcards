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
}