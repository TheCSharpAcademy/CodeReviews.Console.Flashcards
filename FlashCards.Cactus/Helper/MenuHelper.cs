using ConsoleTableExt;

namespace FlashCards.Cactus.Helper
{
    public class MenuHelper
    {
        public static void PrintMenu(string name, List<string> menu)
        {
            Console.Clear();

            ConsoleTableBuilder
            .From(menu)
            .WithTitle(name, ConsoleColor.Yellow, ConsoleColor.DarkGray)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);
        }

        public static void PrintFlashCardsManagementMenu()
        {
            List<string> flashcardMenu = new List<string>
                {
                    "<0> Back to Main menu.",
                    "<1> Show all flashcards.",
                    "<2> Add a new flashcard.",
                    "<3> Delete a flashcard.",
                    "<4> Modify a flashcard.",
                };

            PrintMenu("FlashCard Menu", flashcardMenu);
        }

        public static void PrintMainMenu()
        {
            List<string> menuData = new List<string>
            {
                "<0> Exit app.",
                "<1> Manage Stacks.",
                "<2> Manage FlashCards.",
                "<3> Study.",
                "<4> Study report."
            };
            PrintMenu("Main Menu", menuData);
        }

        public static void PrintStackManagementMenu()
        {
            List<string> stackMenu = new List<string>
                {
                    "<0> Back to Main menu.",
                    "<1> Show Stacks.",
                    "<2> Add a new Stack.",
                    "<3> Delete a Stack.",
                };

            PrintMenu("Stack Menu", stackMenu);
        }

        public static void PrintStudyManagementMenu()
        {
            List<string> studyMenu = new List<string>
                {
                    "<0> Back to Main menu.",
                    "<1> Show all study sessions.",
                    "<2> Start a new study session.",
                    "<3> Delete a study session.",
                };

            PrintMenu("Study Menu", studyMenu);
        }

        public static void PrintStudyReportManagementMenu()
        {
            List<string> studyReportMenu = new List<string>
                {
                    "<0> Back to Main menu.",
                    "<1> Show  the study report from a specific year.",
                };

            PrintMenu("StudyReport Menu", studyReportMenu);
        }
    }
}
