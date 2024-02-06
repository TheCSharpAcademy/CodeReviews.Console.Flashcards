namespace FlashCards.Cactus
{
    public class Application
    {
        public void run()
        {
            while (true)
            {
                PrintMainMenu();
                string? op = Console.ReadLine();
                switch (op)
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    case "1":
                        ManageStacks();
                        break;
                    default:
                        break;
                }
            }
        }

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

        public void ManageStacks()
        {
            while (true)
            {
                PrintStackManagementMenu();

                string? op = Console.ReadLine();
                switch (op)
                {
                    case "0":
                        return;
                    case "1":
                        ShowStacks();
                        break;
                    case "2":
                        AddStack();
                        break;
                    case "3":
                        DeleteStack();
                        break;
                    default:
                        break;
                }
            }
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
