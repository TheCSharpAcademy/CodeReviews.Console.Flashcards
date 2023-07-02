using ConsoleTableExt;

namespace Flashcards.JsPeanut
{
    class Visualization
    {
        public static void DisplayFlashcards(List<FlashcardDto> list)
        {
            ConsoleTableBuilder.From(list)
               .WithFormat(ConsoleTableBuilderFormat.Alternative)
               .ExportAndWriteLine();
        }
        public static void DisplayStacks(List<Stack> list)
        {
            ConsoleTableBuilder.From(list)
               .WithFormat(ConsoleTableBuilderFormat.Alternative)
               .ExportAndWriteLine();
        }
        public static void DisplayStudySessions(List<StudySession> list)
        {
            ConsoleTableBuilder.From(list)
               .WithFormat(ConsoleTableBuilderFormat.Alternative)
               .ExportAndWriteLine();
        }

        public static void DisplayPivotQuery(List<PivotQuery> list)
        {
            ConsoleTableBuilder.From(list)
               .WithFormat(ConsoleTableBuilderFormat.Alternative)
               .ExportAndWriteLine();
        }
    }
}
