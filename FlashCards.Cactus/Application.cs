namespace FlashCards.Cactus
{
    public class Application
    {
        #region Constants

        private const string EXIT_APP = "0";
        private const string MANAGE_STACKS = "1";
        private const string MANAGE_FLASHCARDS = "2";
        private const string STUDY = "3";
        private const string STUDY_REPORT = "4";

        private const string BACK_TO_MAIN = "0";

        private const string SHOW_STACKS = "1";
        private const string ADD_STACK = "2";
        private const string DELETE_STACK = "3";

        private const string SHOW_FLASHCARDS = "1";
        private const string ADD_FLASHCARD = "2";
        private const string DELETE_FLASHCARD = "3";
        private const string MODIFY_FLASHCARD = "4";

        private const string SHOW_STUDYS = "1";
        private const string START_EXISTING_STUDY = "2";
        private const string START_NEW_STUDY = "3";
        private const string DELETE_STUDY = "4";

        private const string SHOW_STUDY_REPORT = "1";

        #endregion Constants

        #region Menu
        public void run()
        {
            while (true)
            {
                PrintMainMenu();
                string? op = Console.ReadLine();
                switch (op)
                {
                    case EXIT_APP:
                        Environment.Exit(0);
                        break;
                    case MANAGE_STACKS:
                        RunStacksManagement();
                        break;
                    case MANAGE_FLASHCARDS:
                        RunFlashCardsManagement();
                        break;
                    case STUDY:
                        RunStudyManagement();
                        break;
                    case STUDY_REPORT:
                        RunStudyReportManagement();
                        break;
                    default:
                        break;
                }
            }
        }

        public void RunStacksManagement()
        {
            while (true)
            {
                PrintStackManagementMenu();

                string? op = Console.ReadLine();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_STACKS:
                        ShowStacks();
                        break;
                    case ADD_STACK:
                        AddStack();
                        break;
                    case DELETE_STACK:
                        DeleteStack();
                        break;
                    default:
                        break;
                }
            }
        }

        public void RunFlashCardsManagement()
        {
            while (true)
            {
                PrintFlashCardsManagementMenu();

                string? op = Console.ReadLine();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_FLASHCARDS:
                        Console.WriteLine("Show all flashcards.");
                        break;
                    case ADD_FLASHCARD:
                        Console.WriteLine("Add a flashcard.");
                        break;
                    case DELETE_FLASHCARD:
                        Console.WriteLine("Delete a flashcard.");
                        break;
                    case MODIFY_FLASHCARD:
                        Console.WriteLine("Modify a flashcard.");
                        break;
                    default:
                        break;
                }
            }
        }

        public void RunStudyManagement()
        {
            while (true)
            {
                PrintStudyManagementMenu();

                string? op = Console.ReadLine();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_STUDYS:
                        Console.WriteLine("Show all study sessions.");
                        break;
                    case START_EXISTING_STUDY:
                        Console.WriteLine("Start from an existing study session.");
                        break;
                    case START_NEW_STUDY:
                        Console.WriteLine("Start a new study session.");
                        break;
                    case DELETE_STUDY:
                        Console.WriteLine("Delete a study session.");
                        break;
                    default:
                        break;
                }
            }
        }

        public void RunStudyReportManagement()
        {
            while (true)
            {
                PrintStudyReportManagementMenu();

                string? op = Console.ReadLine();
                switch (op)
                {
                    case BACK_TO_MAIN:
                        return;
                    case SHOW_STUDY_REPORT:
                        Console.WriteLine("Show  the study report from a specific year.");
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion Menu

        #region Menu print

        private static void PrintMainMenu()
        {
            List<string> menuData = new List<string>
            {
                "<0> Exit app.",
                "<1> Manage Stacks.",
                "<2> Manage FlashCards.",
                "<3> Study.",
                "<4> Study report."
            };
            MenuUtils.PrintMenu("Main Menu", menuData);
        }

        private void PrintStackManagementMenu()
        {
            List<string> stackMenu = new List<string>
                {
                    "<0> Back to Main menu.",
                    "<1> Show Stacks.",
                    "<2> Add a new Stack.",
                    "<3> Delete a Stack.",
                };

            MenuUtils.PrintMenu("Stack Menu", stackMenu);
        }

        private void PrintFlashCardsManagementMenu()
        {
            List<string> flashcardMenu = new List<string>
                {
                    "<0> Back to Main menu.",
                    "<1> Show all flashcards.",
                    "<2> Add a new flashcard.",
                    "<3> Delete a flashcard.",
                    "<4> Modify a flashcard.",
                };

            MenuUtils.PrintMenu("FlashCard Menu", flashcardMenu);
        }

        private void PrintStudyManagementMenu()
        {
            List<string> studyMenu = new List<string>
                {
                    "<0> Back to Main menu.",
                    "<1> Show all study sessions.",
                    "<2> Start from an existing study session.",
                    "<3> Start a new study session.",
                    "<4> Delete a study session.",
                };

            MenuUtils.PrintMenu("Study Menu", studyMenu);
        }

        private void PrintStudyReportManagementMenu()
        {
            List<string> studyReportMenu = new List<string>
                {
                    "<0> Back to Main menu.",
                    "<1> Show  the study report from a specific year.",
                };

            MenuUtils.PrintMenu("StudyReport Menu", studyReportMenu);
        }

        #endregion Menu print

        private void DeleteStack()
        {
            throw new NotImplementedException();
        }

        private void AddStack()
        {
            throw new NotImplementedException();
        }

        private void ShowStacks()
        {
            throw new NotImplementedException();
        }
    }
}
