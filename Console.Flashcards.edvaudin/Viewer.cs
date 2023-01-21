using ConsoleTableExt;
using Flashcards.Models;
namespace Flashcards
{
    static class Viewer
    {
        private static DataAccessor dataAccessor = new();
        public static void DisplayOptionsMenu()
        {
            Console.WriteLine("\nChoose an action from the following list:");
            Console.WriteLine("\ts - Study");
            Console.WriteLine("\tv - View study sessions");
            Console.WriteLine("\ta - Add new stack or flashcard");
            Console.WriteLine("\td - Delete a stack or flashcard");
            Console.WriteLine("\tu - Update a stack or flashcard");
            Console.WriteLine("\t0 - Quit this application");
            Console.Write("Your option? ");
        }

        public static void DisplayTitle()
        {
            Console.WriteLine("Flashcards");
            Console.WriteLine("+-+-+-+-+-+-+-+-+");
        }

        public static List<FlashcardDTO>? ViewFlashcards(string stackName)
        {
            List<FlashcardDTO> flashcards = dataAccessor.GetFlashcardsInStack(stackName);
            if (flashcards.Count == 0 || flashcards == null) { return null; }
            var tableData = new List<List<object>>();
            foreach (FlashcardDTO flashcard in flashcards)
            {
                tableData.Add(new List<object> { flashcards.IndexOf(flashcard) + 1, flashcard.Prompt, flashcard.Answer });
            }
            ConsoleTableBuilder.From(tableData).WithTitle($"Flashcards in {stackName}").WithColumn("Id", "Prompt", "Answer").ExportAndWriteLine();
            return flashcards;
        }

        public static List<StackDTO>? ViewStacks()
        {
            List<StackDTO> stacks = dataAccessor.GetStacks();
            if (stacks.Count == 0 || stacks == null) { return null; }
            var tableData = new List<List<object>>();
            foreach (StackDTO stack in stacks)
            {
                tableData.Add(new List<object> { stacks.IndexOf(stack) + 1, stack.Name });
            }
            ConsoleTableBuilder.From(tableData).WithTitle("Your Stacks").WithColumn("Id", "Name").ExportAndWriteLine();
            return stacks;
        }

        public static void ViewStudies()
        {
            List<StudyDTO> studies = dataAccessor.GetStudies();
            var tableData = new List<List<object>>();
            foreach (StudyDTO study in studies)
            {
                tableData.Add(new List<object> { study.Stack, Math.Round(study.Score * 100, 2), study.Date });
            }
            ConsoleTableBuilder.From(tableData).WithTitle("Your Studies").WithColumn("Stack", "Score (%)", "Date").ExportAndWriteLine();
        }
    }
}
