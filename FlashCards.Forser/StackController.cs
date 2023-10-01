namespace FlashCards.Forser
{
    internal class StackController
    {
        private readonly DataLayer _dataLayer = new DataLayer();
        internal void ShowStackMenu()
        {
            FlashcardController flashcardController = new FlashcardController();
            MainMenuController mainMenuController = new MainMenuController();

            AnsiConsole.Clear();
            Menu.RenderTitle("Stack Menu");
            int selectedStackMenu = AnsiConsole.Prompt(DrawMenu()).Id;

            switch (selectedStackMenu)
            {
                case 0:
                    ListAllStacks();
                    break;
                case 1:
                    AddNewStack();
                    break;
                case 2:
                    break;
                case 3:
                    DeleteStack();
                    break;
                case 4:
                    flashcardController.ShowFlashcardMenu();
                    break;
                case 5:
                    mainMenuController.MainMenu();
                    break;
                default:
                    AnsiConsole.WriteLine("Not a valid option, select from an option from the Menu");
                    break;
            }
        }
        private void DeleteStack()
        {
            List<Stack> allStacks = _dataLayer.FetchAllStacks();
            bool stackDeleted = false;

            Table table = new();
            table.AddColumn("Id");
            table.AddColumn("Stack Name");

            foreach(Stack stack in allStacks)
            {
                table.AddRow($"{stack.StackId}", $"{stack.Name}");
            }
            AnsiConsole.Write(table);
            int stackId = AnsiConsole.Ask<int>("Enter the ID of the stack you want to remove: ");

            if (_dataLayer.CheckStackId(stackId))
            {
                stackDeleted = _dataLayer.DeleteStackById(stackId);
            }

            if(stackDeleted) 
            {
                AnsiConsole.WriteLine($"Stack with ID : {stackId} has been deleted!");
            }
            else
            {
                AnsiConsole.WriteLine("No stack deleted.");
            }
            AnsiConsole.WriteLine("Press any key to return to Stack Menu");
            Console.ReadLine();
            ShowStackMenu();
        }
        internal void AddNewStack()
        {
            AnsiConsole.Clear();
            Menu.RenderTitle("Add a new Stack");
            string stackName = AnsiConsole.Ask<string>("Enter your new [blue]Stack name[/]:");

            Stack newStack = new Stack(name: stackName);
            int rows = _dataLayer.NewStackEntry(newStack);

            if (rows > 0)
            {
                AnsiConsole.WriteLine("Your new stack has been saved.");
            }
            else
            {
                AnsiConsole.WriteLine($"Stack with the name {newStack.Name} didn't get saved");
            }
            AnsiConsole.WriteLine("Press any key to return to Stack Menu");
            Console.ReadLine();
            ShowStackMenu();
        }
        internal int CountAllStacks()
        {
            return _dataLayer.ReturnNumberOfStacks();
        }
        public void ListAllStacks()
        {
            List<Stack> allStacks = _dataLayer.FetchAllStacks();

            Table table = new Table();
            table.Expand();
            table.AddColumn("Stack Name");

            if (allStacks.Any())
            {
                foreach (Stack stack in allStacks)
                {
                    table.AddRow($"{stack.Name}");
                }
                AnsiConsole.Write(table);
            }
            else
            {
                AnsiConsole.WriteLine($"Found no stacks!");
            }
            AnsiConsole.WriteLine("Press any key to return to Stack Menu");
            Console.ReadLine();
            ShowStackMenu();
        }
        private SelectionPrompt<Menu> DrawMenu()
        {
            SelectionPrompt<Menu> menu = new()
            {
                HighlightStyle = Menu.HighLightStyle
            };

            List<Menu> stackMenu = new List<Menu>();
            stackMenu.Add(new Menu { Id = 0, Text = "List all Stacks" });
            stackMenu.Add(new Menu { Id = 1, Text = "Add new Stack" });
            if (CountAllStacks() > 0)
            {
                stackMenu.Add(new Menu { Id = 2, Text = "Edit a Stack" });
                stackMenu.Add(new Menu { Id = 3, Text = "Delete a Stack" });
            }
            stackMenu.Add(new Menu { Id = 4, Text = "Go to Flashcard Menu" });
            stackMenu.Add(new Menu { Id = 5, Text = "Return to Main Menu" });

            menu.Title("Select an [B]option[/]");
            menu.AddChoices(stackMenu);

            return menu;
        }
    }
}