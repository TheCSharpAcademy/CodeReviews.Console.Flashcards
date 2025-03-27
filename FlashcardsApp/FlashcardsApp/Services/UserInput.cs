//using FlashcardsApp.DTOs;
//using FlashcardsApp.Models;
//using Spectre.Console;

//namespace FlashcardsApp.Services
//{
//    internal class UserInput
//    {
//        private readonly DatabaseService _databaseService;

//        public UserInput(DatabaseService databaseService)
//        {
//            _databaseService = databaseService;
//        }

//        internal void MainMenu()
//        {
//            Console.Clear();
//            bool closeApp = false;
//            while (closeApp == false)
//            {
//                var choice = AnsiConsole.Prompt(
//                    new SelectionPrompt<string>()
//                        .Title("MAIN MENU")
//                        .AddChoices(new[] {
//                            "View All Stacks",
//                            "Create New Stack",
//                            "Manage Stack",
//                            "Study a Stack",
//                            "View Study History",
//                            "Close Application"
//                        }));

//                switch (choice)
//                {
//                    case "View All Stacks":
//                        _databaseService.GetAllStacks();
//                        break;
//                    case "Create New Stack":
//                        CreateStack();
//                        break;
//                    case "Manage Stack":
//                        StacksListMenu();
//                        break;
//                    case "Study a Stack":
//                        break;
//                    case "View Study History":
//                        break;
//                    case "Close Application":
//                        closeApp = true;
//                        Environment.Exit(0);
//                        break;
//                }
//            }
//        }

//        internal void StacksListMenu()
//        {
//            Console.Clear();
//            _databaseService.GetAllStacks();

//            List<Stack> stacks = _databaseService.GetAllStacksAsList();
//            List<string> stackNames = stacks.Select(s => s.Name).ToList();
//            stackNames.Add("Return to Main Menu");

//            var selectedStack = AnsiConsole.Prompt(
//                new SelectionPrompt<string>()
//                .Title("Select a stack to manage.")
//                .AddChoices(stackNames));
//            if (selectedStack == "Return to Main Menu")
//            {
//                MainMenu();
//            }

//            while (true)
//            {
//                Console.Clear();

//                var action = AnsiConsole.Prompt(
//                    new SelectionPrompt<string>()
//                    .Title($"Managing stack: {selectedStack}")
//                    .AddChoices(new[]
//                    {
//                    "View Cards",
//                    "Add Card",
//                    "Update Card",
//                    "Delete Card",
//                    "Delete this Stack",
//                    "Return to Stack Menu",
//                    "Return to Main Menu"
//                    }));

//                int stackId = GetStackId(selectedStack);

//                switch (action)
//                {
//                    case "View Cards":
//                        _databaseService.GetFlashcardsByID(stackId);
//                        break;
//                    case "Add Card":
//                        CreateFlashcard(stackId);
//                        break;
//                    case "Update Card":
//                        UpdateFlashcard(stackId);
//                        break;
//                    case "Delete Card":
//                        DeleteFlashcard(stackId);
//                        break;
//                    case "Delete this Stack":
//                        DeleteStack(stackId, selectedStack);
//                        break;
//                    case "Return to Stack Menu":
//                        StacksListMenu();
//                        return;
//                    case "Return to Main Menu":
//                        MainMenu();
//                        return;
//                }
//                Console.WriteLine("\n\nPress Enter...");
//                Console.ReadLine();
//            }
//        }

//        private void UpdateFlashcard(int stackId)
//        {
//            List<FlashcardDTO> flashcards = _databaseService.GetFlashcardsByID(stackId);

//            Dictionary<string, int> cardMapping = flashcards.ToDictionary(
//                f => $"Front: {f.Front}\t\t| Back: {f.Back}",
//                f => f.FlashcardId);

//            var choices = cardMapping.Keys.ToList();
//            choices.Add("Return to Stack Menu");

//            var selectedCard = AnsiConsole.Prompt(
//                new SelectionPrompt<string>()
//                .Title("Select a card to update.")
//                .AddChoices(choices));
//            if (selectedCard == "Return to Stack Menu")
//            {
//                StacksListMenu();
//                return;
//            }

//            int flashcardId = cardMapping[selectedCard];

//            Flashcard? flashcard = _databaseService.GetFlashcardByFlashcardId(flashcardId, stackId);
//            if (flashcard == null)
//            {
//                return;
//            }

//            bool finished = false;
//            while (finished == false)
//            {
//                var choice = AnsiConsole.Prompt(
//                    new SelectionPrompt<string>()
//                    .Title("Select what to update, save, or exit.")
//                    .AddChoices(new[]
//                    {
//                        "Front",
//                        "Back",
//                        "Save Changes",
//                        "Exit to Stack Menu",
//                        "Exit to Main Menu"
//                    }));

//                switch (choice)
//                {
//                    case "Front":
//                        flashcard.Front = GetFrontFlashcard();
//                        break;
//                    case "Back":
//                        flashcard.Back = GetBackFlashcard();
//                        break;
//                    case "Save Changes":
//                        flashcard.CreatedDate = DateTime.Now;
//                        _databaseService.UpdateFlashcard(stackId, flashcardId, flashcard);
//                        finished = true;
//                        break;
//                    case "Exit to Stack Menu":
//                        StacksListMenu();
//                        return;
//                    case "Exit to Main Menu":
//                        MainMenu();
//                        return;
//                }
//            }

//        }

//        private void DeleteStack(int stackId, string selectedStack)
//        {
//            var action = AnsiConsole.Prompt(
//                    new SelectionPrompt<string>()
//                    .Title($"\nAre you sure you want to delete '{selectedStack}' stack?")
//                    .AddChoices(new[]
//                    {
//                    "Yes",
//                    "No"
//                    }));

//            switch (action)
//            {
//                case "Yes":
//                    _databaseService.DeleteStack(stackId);
//                    Console.WriteLine("\n\nPress Enter...");
//                    Console.ReadLine();
//                    StacksListMenu();
//                    return;
//                case "No":
//                    return;
//            }
//        }

//        private void DeleteFlashcard(int stackId)
//        {
//            List<FlashcardDTO> flashcards = _databaseService.GetFlashcardsByID(stackId);

//            Dictionary<string, int> cardMapping = flashcards.ToDictionary(
//                f => $"Front: {f.Front}\t\t| Back: {f.Back}",
//                f => f.FlashcardId
//                );
//            var choices = cardMapping.Keys.ToList();
//            choices.Add("Return to Stack Menu");

//            var selectedCard = AnsiConsole.Prompt(
//                new SelectionPrompt<string>()
//                .Title("Select a card to delete.")
//                .AddChoices(choices));
//            if (selectedCard == "Return to Stack Menu")
//            {
//                StacksListMenu();
//                return;
//            }

//            int flashcardId = cardMapping[selectedCard];
//            _databaseService.DeleteFlashcard(stackId, flashcardId);
//        }

//        private int GetStackId(string selectedStack)
//        {
//            List<Stack> stacks = _databaseService.GetAllStacksAsList();
//            Stack stack = stacks.First(s => s.Name == selectedStack);
//            return stack.StackId;
//        }

//        private void CreateFlashcard(int stackId)
//        {
//            Flashcard flashcard = new();

//            string front = GetFrontFlashcard();

//            string back = GetBackFlashcard();

//            flashcard.StackId = stackId;
//            flashcard.Front = front;
//            flashcard.Back = back;
//            flashcard.CreatedDate = DateTime.Now;

//            _databaseService.PostFlashcard(flashcard);
//        }

//        internal string GetFrontFlashcard()
//        {
//            Console.Write("\n\nEnter the content for the front of the card OR 0 to return to Stack Menu: ");
//            string? front = Console.ReadLine();

//            if (front == "0") StacksListMenu();

//            while (string.IsNullOrEmpty(front) || front.Length > 500)
//            {
//                Console.WriteLine("\nFront cannot be empty or longer than 500 characcters.");
//                Console.Write("\nEnter the front: ");
//                front = Console.ReadLine();
//            }

//            return front;
//        }

//        internal string GetBackFlashcard()
//        {
//            Console.Write("\n\nEnter the content for the back of the card OR 0 to return to Stack Menu: ");
//            string? back = Console.ReadLine();

//            if (back == "0") StacksListMenu();

//            while (string.IsNullOrEmpty(back) || back.Length > 500)
//            {
//                Console.WriteLine("\nBack cannot be empty or longer than 500 characcters.");
//                Console.Write("\nEnter the back: ");
//                back = Console.ReadLine();
//            }

//            return back;
//        }

//        private void CreateStack()
//        {
//            Stack stack = new();

//            Console.Write("\nEnter a name for the stack of flashcards OR 0 to return to Main Menu: ");
//            string? name = Console.ReadLine();

//            if (name == "0") MainMenu();

//            while (string.IsNullOrEmpty(name) || name.Length > 100)
//            {
//                Console.WriteLine("\nName cannot be empty or longer than 100 characters.");
//                Console.Write("\n\nPlease enter a valid name: ");
//                name = Console.ReadLine();
//            }

//            Console.Write("\nIf you would like to add a description type it here or press enter: ");
//            string? description;

//            do
//            {
//                description = Console.ReadLine();

//                if (string.IsNullOrWhiteSpace(description))
//                {
//                    description = "No description provided";
//                }

//                if (description.Length > 500)
//                {
//                    Console.WriteLine("\nSorry description cannot be longer than 500 characters");
//                    Console.Write("\nEnter description again: ");
//                }
//            } while (description.Length > 500);

//            stack.Name = name;
//            stack.Description = description;
//            stack.CreatedDate = DateTime.Now;

//            _databaseService.Post(stack);
//        }
//    }
//}