using ConsoleTableExt;

namespace Flashcards.jkjones98;

internal class ShowTable
{
    internal static void CreateStackTable<T>(List<T> stackTable) where T : class
    {
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(stackTable)
            .WithTitle("Language Stacks")
            .ExportAndWriteLine();
        Console.WriteLine("\n\n");
    }

    internal static void CreateFlashcardTable<T>(List<T> flashcardTable) where T : class
    {
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(flashcardTable)
            .WithTitle("Flashcards")
            .ExportAndWriteLine();
        Console.WriteLine("\n\n");
    }

    internal static void CreateSessionTable<T>(List<T> studyFlashcard) where T : class
    {
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(studyFlashcard)
            .WithTitle("Flashcard")
            .ExportAndWriteLine();
        Console.WriteLine("\n\n");
    }

    internal static void CreateStudyTable<T>(List<T> studyTable) where T : class
    {
        Console.WriteLine("\n\n");

        ConsoleTableBuilder
            .From(studyTable)
            .WithTitle("Session Data")
            .ExportAndWriteLine();
        Console.WriteLine("\n\n");
    }
}