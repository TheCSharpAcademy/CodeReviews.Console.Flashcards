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
                        ManageStacksMenu();
                        break;
                    case MANAGE_FLASHCARDS:
                        Console.WriteLine("Manage FlashCards.");
                        break;
                    case STUDY:
                        Console.WriteLine("Study");
                        break;
                    case STUDY_REPORT:
                        Console.WriteLine("Study Report");
                        break;
                    default:
                        break;
                }
            }
        }

        public void ManageStacksMenu()
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
