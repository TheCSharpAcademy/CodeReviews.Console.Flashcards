namespace FlashCards.Forser
{
    internal class StackController
    {
        private readonly DataLayer _dataLayer = new DataLayer();
        internal void ShowStackMenu()
        {
            FlashcardController flashcardController = new FlashcardController();
            MainMenuController mainMenuController = new MainMenuController();

            Console.Clear();
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("              STACK MENU");
            Console.WriteLine("List - List all Stacks");
            Console.WriteLine("Add - Add a new Stack");
            if (CountAllStacks() >= 1)
            {
                Console.WriteLine("Edit - Edit a Stack");
                Console.WriteLine("Delete - Delete a Stack\n");
            }
            Console.WriteLine("Flash - Go to Flashcard Menu");
            Console.WriteLine("Menu - Return to Main Menu");
            Console.WriteLine("------------------------------------------");

            string selectedStackMenu = Console.ReadLine().Trim().ToLower();

            switch (selectedStackMenu)
            {
                case "list":
                    ListAllStacks();
                    break;
                case "add":
                    AddNewStack();
                    break;
                case "edit":
                    break;
                case "delete":
                    DeleteStack();
                    break;
                case "menu":
                    mainMenuController.MainMenu();
                    break;
                case "flash":
                    flashcardController.ShowFlashcardMenu();
                    break;
                default:
                    Console.WriteLine("Not a valid option, select from an option from the Menu");
                    break;
            }
        }
        private void DeleteStack()
        {
            List<Stack> allStacks = _dataLayer.FetchAllStacks();
            bool stackDeleted = false;

            foreach(Stack stack in allStacks)
            {
                Console.WriteLine($"ID: {stack.StackId} - Name: {stack.Name}");
            }
            Console.WriteLine("Enter the ID of the stack you want to remove: ");
            int stackId = Convert.ToInt32(Console.ReadLine());

            if (_dataLayer.CheckStackId(stackId))
            {
                stackDeleted = _dataLayer.DeleteStackById(stackId);
            }

            if(stackDeleted) 
            {
                Console.WriteLine($"Stack with ID : {stackId} has been deleted!\n Press any key to return to Stack Menu");
                Console.ReadLine();
                ShowStackMenu();
            }
            else
            {
                Console.WriteLine("No stack deleted.\n Press any key to return to Stack Menu");
                ShowStackMenu();
            }
        }
        internal void AddNewStack()
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("Enter your new Stack name: ");
            string stackName = Console.ReadLine().Trim();

            Stack newStack = new Stack(name: stackName);
            int rows = _dataLayer.NewStackEntry(newStack);

            if (rows > 0)
            {
                Console.WriteLine("Stack has been saved.");
                Console.WriteLine("Press ENTER to return to Stack Menu");
                Console.ReadLine();
                ShowStackMenu();
            }
            Console.WriteLine($"Stack with the name {newStack.Name} didn't get saved");
            Console.WriteLine("Press any key to return to Stack Menu");
            Console.ReadLine();
            ShowStackMenu();
        }
        internal int CountAllStacks()
        {
            int stackCount = _dataLayer.ReturnNumberOfStacks();

            return stackCount;
        }
        public void ListAllStacks()
        {
            List<Stack> allStacks = _dataLayer.FetchAllStacks();
            if (allStacks.Any())
            {
                Console.WriteLine($"Found {allStacks.Count()} Stacks");
                foreach (Stack stack in allStacks)
                {
                    Console.WriteLine($"{stack.Name}");
                }
                Console.WriteLine("Press any key to return to Stack Menu");
                Console.ReadLine();
                ShowStackMenu();
            }

            Console.WriteLine($"Found no stacks! Press any key to return to Stack Menu");
            Console.ReadLine();
            ShowStackMenu();
        }
    }
}