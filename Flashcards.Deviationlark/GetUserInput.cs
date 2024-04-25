namespace Flashcards
{
    internal class GetUserInput
    {
        public string? userInput;
        public static Validation validation = new Validation();
        public static FlashcardsController flashcardsController = new();
        public static StacksController stacksController = new();
        public List<StackModel> stacks = new();
        internal void MainMenu()
        {
            Console.Clear();
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("--------------------");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Manage Stacks");
            Console.WriteLine("2. Manage Flashcards");
            Console.WriteLine("3. Study");
            Console.WriteLine("Choose an option: ");
            userInput = Console.ReadLine();

            switch (userInput)
            {
                case "0":
                    Console.WriteLine("Bye!");
                    Environment.Exit(0);
                    break;
                case "1":
                    Stacks();
                    break;
                case "2":
                    Flashcards();
                    break;
                case "3":
                    Study();
                    break;
                default:
                    Console.WriteLine("That's not an option.");
                    Console.WriteLine("Press enter to go back to Main Menu.");
                    Console.ReadLine();
                    MainMenu();
                    break;
            }
        }

        internal void Stacks()
        {
            Console.Clear();
            Console.WriteLine("STACKS");
            Console.WriteLine("--------------------");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Main Menu");
            Console.WriteLine("2. View All Stacks");
            Console.WriteLine("3. Create a Stack");
            Console.WriteLine("4. Delete a Stack");
            Console.WriteLine("5. Edit a Stack");
            Console.WriteLine("Choose an option: ");
            userInput = Console.ReadLine();

            switch (userInput)
            {
                case "0":
                    Console.WriteLine("Bye!");
                    Environment.Exit(0);
                    break;
                case "1":
                    MainMenu();
                    break;
                case "2":
                    stacks = stacksController.ViewStacks();
                    if (stacks.Count == 0) validation.ToMainMenu("No stacks in database");
                    Console.WriteLine("Press enter to go back to main menu.");
                    Console.ReadLine();
                    MainMenu();
                    break;
                case "3":
                    CreateStack();
                    break;
                case "4":
                    DeleteStack();
                    break;
                case "5":
                    EditStack();
                    break;
                default:
                    Console.WriteLine("That's not an option.");
                    Console.WriteLine("Press enter to go back to Stacks menu.");
                    Console.ReadLine();
                    Stacks();
                    break;

            }
        }

        internal void CreateStack()
        {
            StackModel stackModel = new();
            stacks = stacksController.ViewStacks();

            Console.WriteLine("Enter the name of the stack you want to create: ");
            userInput = Console.ReadLine();
            while (int.TryParse(userInput, out _) || string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Name can't be a number or empty.");
                Console.WriteLine("Enter the name of the stack you want to create: ");
                userInput = Console.ReadLine();
            }

            foreach (var stack in stacks)
            {
                while (userInput.ToLower() == stack.Name.ToLower())
                {
                    Console.WriteLine($"{stack.Name} already exists. Enter a different name: ");
                    userInput = Console.ReadLine();
                }
            }

            stackModel.Name = userInput;

            stacksController.InsertStack(stackModel);
            MainMenu();

        }
        internal void EditStack()
        {
            string message = "Type the id of the stack you want to update: ";
            StackModel stack = new();
            List<StackModel> stacks = stacksController.ViewStacks();
            int stackId = GetNumInput(message, "stacks");

            while (stackId < 1 || stackId > stacks.Count)
            {
                if (stackId < 1)
                    stackId = validation.LessThanOne(message, stackId, 0);
                if (stackId > stacks.Count)
                    stackId = validation.BiggerThanList(message, stackId, 0);
            }

            Console.WriteLine("Enter the name of the stack you want to create: ");
            userInput = Console.ReadLine();
            while (int.TryParse(userInput, out _) || string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine("Name can't be a number or empty.");
                Console.WriteLine("Enter the name of the stack you want to create: ");
                userInput = Console.ReadLine();
            }

            foreach (var record in stacks)
            {
                while (userInput.ToLower() == record.Name.ToLower())
                {
                    Console.WriteLine($"{record.Name} already exists. Enter a different name: ");
                    userInput = Console.ReadLine();
                }
            }

            stack.Name = userInput;
            int rowCount = stacksController.UpdateStack(stacks[stackId - 1].StackId, stack);

            if (rowCount == 0) validation.ToMainMenu("There is no stacks in the database.");

            Console.WriteLine("Stack Updated. Press enter to go back to main menu.");
            Console.ReadLine();
            MainMenu();
        }

        internal void DeleteStack()
        {
            string message = "Type the id of the stack you want to delete: ";
            List<StackModel> stacks = stacksController.ViewStacks();
            int stackId = GetNumInput(message, "stacks");
            while (stackId < 1 || stackId > stacks.Count)
            {
                if (stackId < 1)
                    stackId = validation.LessThanOne(message, stackId, 0);
                if (stackId > stacks.Count)
                    stackId = validation.BiggerThanList(message, stackId, 0);
            }

            int rowCount = stacksController.RemoveStack(stacks[stackId - 1].StackId);

            if (rowCount == 0) validation.ToMainMenu("There is no stacks in the database.");

            Console.WriteLine("Stack deleted. Press enter to go back to main menu.");
            Console.ReadLine();
            MainMenu();
        }

        internal int GetNumInput(string message, string table, int stackId = 0)
        {
            Console.WriteLine(message);
            Console.WriteLine("Type 0 to go back to main menu.");
            userInput = Console.ReadLine();

            while (!int.TryParse(userInput, out _) || string.IsNullOrEmpty(userInput))
            {
                Console.Clear();
                Console.WriteLine("Incorrect format.");
                if (table == "stacks")
                    stacksController.ViewStacks();
                if (table == "flashcards")
                    flashcardsController.ReadFlashcards(stackId);
                Console.WriteLine(message);
                Console.WriteLine("Type 0 to go back to main menu.");
                userInput = Console.ReadLine();
            }
            int number = int.Parse(userInput);
            if (number == 0) MainMenu();
            return number;
        }

        internal void Flashcards()
        {
            Console.Clear();
            Console.WriteLine("FLASHCARDS");
            Console.WriteLine("--------------------");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Back to main menu");
            Console.WriteLine("2. View All Flashcards");
            Console.WriteLine("3. Create a Flashcard");
            Console.WriteLine("4. Delete a Flashcard");
            Console.WriteLine("5. Edit a Flashcard");
            Console.WriteLine("Choose an option: ");
            userInput = Console.ReadLine();

            switch (userInput)
            {
                case "0":
                    Console.WriteLine("Bye!");
                    Environment.Exit(0);
                    break;
                case "1":
                    MainMenu();
                    break;
                case "2":
                    ViewFlashcards();
                    MainMenu();
                    break;
                case "3":
                    CreateFlashcard();
                    MainMenu();
                    break;
                case "4":
                    DeleteFlashcard();
                    break;
                case "5":
                    EditFlashcard();
                    break;
                default:
                    Console.WriteLine("That's not an option.");
                    Console.WriteLine("Press enter to go back to Flashcards Menu.");
                    Console.ReadLine();
                    Flashcards();
                    break;
            }
        }

        internal void ViewFlashcards()
        {
            string message = "Type the id of the stack you want to view the flashcards of: ";
            stacks = stacksController.ViewStacks();
            if (stacks.Count == 0) validation.ToMainMenu("There are no stacks in the database.");

            int stackId = GetNumInput(message, "stacks");
            while (stackId < 1 || stackId > stacks.Count)
            {
                if (stackId < 1)
                    stackId = validation.LessThanOne(message, stackId, 0);
                if (stackId > stacks.Count)
                    stackId = validation.BiggerThanList(message, stackId, 0);
            }

            List<FlashcardModel> flashcardCount = flashcardsController.ReadFlashcards(stacks[stackId - 1].StackId);

            if (flashcardCount.Count == 0) validation.ToMainMenu("No flashcards found.");

            Console.WriteLine("Press enter to go back to main menu.");
            Console.ReadLine();
        }

        internal void CreateFlashcard()
        {
            string message = "Type the id of the stack you want to create a flashcard in: ";
            FlashcardModel flashcard = new();
            stacks = stacksController.ViewStacks();

            if (stacks.Count == 0) validation.ToMainMenu("There is no stacks in the database.");

            int stackId = GetNumInput(message, "stacks");
            while (stackId < 1 || stackId > stacks.Count)
            {
                if (stackId < 1)
                    stackId = validation.LessThanOne(message, stackId, 0);
                if (stackId > stacks.Count)
                    stackId = validation.BiggerThanList(message, stackId, 0);
            }

            Console.WriteLine("Enter the question that will be on the front of the card: ");
            string? front = Console.ReadLine();
            while (string.IsNullOrEmpty(front))
            {
                Console.WriteLine("Question can't be empty.");
                Console.Clear();
                Console.WriteLine("Enter the question that will be on the front of the card: ");
                front = Console.ReadLine();
            }

            Console.WriteLine("Enter the answer that will be on the back of the card: ");
            string? back = Console.ReadLine();
            while (string.IsNullOrEmpty(back))
            {
                Console.WriteLine("Answer can't be empty.");
                Console.Clear();
                Console.WriteLine("Enter the question that will be on the front of the card: ");
                front = Console.ReadLine();
            }
            flashcard.StackId = stacks[stackId - 1].StackId;
            flashcard.Front = front;
            flashcard.Back = back;

            flashcardsController.InsertFlashcard(flashcard);
        }

        internal void EditFlashcard()
        {
            string message1 = "Type the id of the stack you want to update a flashcard of: ";
            string message2 = "Type the id of the flashcard you want to update: ";
            FlashcardModel flashcard = new();
            stacks = stacksController.ViewStacks();
            if (stacks.Count == 0) validation.ToMainMenu("There are no stacks in the database.");

            int stackId = GetNumInput(message1, "stacks");

            while (stackId < 1 || stackId > stacks.Count)
            {
                if (stackId < 1)
                    stackId = validation.LessThanOne(message1, stackId, 0);
                if (stackId > stacks.Count)
                    stackId = validation.BiggerThanList(message1, stackId, 0);
            }

            List<FlashcardModel> flashcards = flashcardsController.ReadFlashcards(stacks[stackId - 1].StackId);
            if (flashcards.Count == 0) validation.ToMainMenu("There are no flashcards in the database.");

            int flashcardId = GetNumInput(message2, "flashcards", stacks[stackId - 1].StackId);
            while (flashcardId < 1 || flashcardId > flashcards.Count)
            {
                if (flashcardId < 1)
                    flashcardId = validation.LessThanOne(message2, stacks[stackId - 1].StackId, 1);
                if (flashcardId > flashcards.Count)
                    flashcardId = validation.BiggerThanList(message2, stacks[stackId - 1].StackId, 1);
            }

            Console.WriteLine("Enter the question that will be on the front of the card: ");
            string? front = Console.ReadLine();
            while (string.IsNullOrEmpty(front))
            {
                Console.WriteLine("Question can't be empty.");
                Console.Clear();
                Console.WriteLine("Enter the question that will be on the front of the card: ");
                front = Console.ReadLine();
            }

            Console.WriteLine("Enter the answer that will be on the back of the card: ");
            string? back = Console.ReadLine();
            while (string.IsNullOrEmpty(back))
            {
                Console.WriteLine("Answer can't be empty.");
                Console.Clear();
                Console.WriteLine("Enter the question that will be on the front of the card: ");
                front = Console.ReadLine();
            }

            flashcard.Front = front;
            flashcard.Back = back;
            flashcard.StackId = stackId;

            int rowCount = flashcardsController.UpdateFlashcard(flashcards[flashcardId - 1].FlashcardId, flashcard);

            if (rowCount == 0) validation.ToMainMenu($"There is no flashcard with id {flashcardId} in the database.");

            Console.WriteLine("Flashcard updated. Press enter to go back to main menu.");
            Console.ReadLine();
            MainMenu();
        }

        internal void DeleteFlashcard()
        {
            string message1 = "Type the id of the stack you want to delete a flashcards of: ";
            string message2 = "Type the id of the flashcard you want to delete: ";
            stacks = stacksController.ViewStacks();
            if (stacks.Count == 0) validation.ToMainMenu("There are no stacks in the database.");

            int stackId = GetNumInput(message1, "stacks");

            while (stackId < 1 || stackId > stacks.Count)
            {
                if (stackId < 1)
                    stackId = validation.LessThanOne(message1, stackId, 0);
                if (stackId > stacks.Count)
                    stackId = validation.BiggerThanList(message1, stackId, 0);
            }

            List<FlashcardModel> flashcards = flashcardsController.ReadFlashcards(stacks[stackId - 1].StackId);

            if (flashcards.Count == 0) validation.ToMainMenu("No flashcards found.");

            int flashcardId = GetNumInput(message2, "flashcards");

            while (flashcardId < 1 || flashcardId > flashcards.Count)
            {
                if (flashcardId < 1)
                    flashcardId = validation.LessThanOne(message2, stacks[stackId - 1].StackId, 1);
                if (flashcardId > flashcards.Count)
                    flashcardId = validation.BiggerThanList(message2, stacks[stackId - 1].StackId, 1);
            }

            int rowCount = flashcardsController.RemoveFlashcard(stacks[stackId - 1].StackId, flashcards[flashcardId - 1].FlashcardId);

            if (rowCount == 0) validation.ToMainMenu("There is no flashcards in the database.");

            Console.WriteLine("Flashcard deleted. Press enter to go back to main menu.");
            Console.ReadLine();
            MainMenu();
        }


        internal void Study()
        {
            StudyController studyController = new();
            Console.Clear();
            Console.WriteLine("STUDY");
            Console.WriteLine("--------------------");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Main Menu");
            Console.WriteLine("2. Study");
            Console.WriteLine("3. View history");
            Console.WriteLine("Choose an option: ");
            userInput = Console.ReadLine();

            switch (userInput)
            {
                case "0":
                    Console.WriteLine("Bye!");
                    Environment.Exit(0);
                    break;
                case "1":
                    MainMenu();
                    break;
                case "2":
                    StudyFlashcards();
                    break;
                case "3":
                    studyController.View();
                    MainMenu();
                    break;
                default:
                    Console.WriteLine("That's not an option.");
                    Console.WriteLine("Press enter to go back to Study menu.");
                    Console.ReadLine();
                    Study();
                    break;

            }
        }

        internal void StudyFlashcards()
        {
            string message = "Type the id of the stack you want to study: ";
            StudyController studyController = new();

            stacks = stacksController.ViewStacks();
            int stackId = GetNumInput(message, "stacks");

            while (stackId < 1 || stackId > stacks.Count)
            {
                if (stackId < 1)
                    stackId = validation.LessThanOne(message, stackId, 0);
                if (stackId > stacks.Count)
                    stackId = validation.BiggerThanList(message, stackId, 0);
            }

            int flashcards = studyController.ReadRandomFlashcards(stacks[stackId - 1].StackId);

            if (flashcards == 0) validation.ToMainMenu("No flashcards found.");

            MainMenu();
        }
    }
}