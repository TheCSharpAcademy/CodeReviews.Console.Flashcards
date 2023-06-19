using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.JsPeanut
{
    class Visualization
    {
        public static void DisplayFlashcards(List<FlashcardDTO> list)
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
