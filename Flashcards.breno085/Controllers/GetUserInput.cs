using flashcards.Repositories;

namespace flashcards.Controllers
{
    public class GetUserInput
    {
        public void MainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Main Menu\n");
                Console.WriteLine("Type to select an option:");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - Manage Stacks");
                Console.WriteLine("2 - Manage Flashcards");
                Console.WriteLine("3 - Study");
                Console.WriteLine("4 - View Study Session Data\n");

                string op = Console.ReadLine();

                switch (op)
                {
                    case "0":
                        exit = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        ManageStacks();
                        break;
                    case "2":
                        FlashcardsMenu();
                        break;
                    case "3":
                        Study();
                        break;
                    case "4":
                        ViewStudySession();
                        break;
                    default:
                        Console.WriteLine("Type a valid option.");
                        break;
                }
            }

        }

        public void ViewStudySession()
        {
            var userRepository = new UserRepository();

            userRepository.ViewStudySessionData();
        }

        public void Study()
        {
            var userRepository = new UserRepository();
            string stackName = GetStackName("study");

            List<int> flashcardsId = userRepository.GetFlashcardsId(stackName);

            int score = 0;

            for (int i = 0; i < flashcardsId.Count; i++)
            {
                string front = userRepository.GetFlashcardsFront(flashcardsId[i]);
                Console.WriteLine("Front");
                Console.WriteLine(front);

                Console.WriteLine("\nInput your answer to this card or 0 to exit:");
                string answer = Console.ReadLine().Trim();

                string back = userRepository.GetFlashcardsBack(flashcardsId[i]);

                if (answer == "0")
                {
                    MainMenu();
                    return;
                }

                if (back == answer)
                {
                    score++;
                    Console.WriteLine("Correct!\n");
                }
                else
                {
                    Console.WriteLine($"Your answered {answer}.");
                    Console.WriteLine("Your answer was wrong.");
                    Console.WriteLine($"The correct answer is {back}\n.");

                    Console.WriteLine("Type any key to continue...");
                    Console.ReadLine();
                }
            }
            Console.WriteLine($"You scored {score}/{flashcardsId.Count}.");
            userRepository.InsertStudyData(stackName, score);
        }

        private void ManageStacks()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Manage Stacks Menu\n");
                Console.WriteLine("Type to select an option:");
                Console.WriteLine("0 - Return to main menu");
                Console.WriteLine("1 - View all stacks");
                Console.WriteLine("2 - Create a stack");
                Console.WriteLine("3 - Delete a stack");

                string op = Console.ReadLine();

                switch (op)
                {
                    case "0":
                        MainMenu();
                        break;
                    case "1":
                        ViewStacks();
                        break;
                    case "2":
                        InsertStacks();
                        break;
                    case "3":
                        DeleteStacks();
                        break;
                    default:
                        Console.WriteLine("Type a valid option.");
                        break;
                }
            }
        }

        private void ViewStacks()
        {
            GetStacks();

            Console.WriteLine("\nType any key to exit\n");
            Console.ReadLine();
        }

        private void DeleteStacks()
        {
            var userRepository = new UserRepository();
            string stack = GetStackName("delete");

            if (stack != "0")
                userRepository.DeleteStackData(stack);
        }

        private void InsertStacks()
        {
            var userRepository = new UserRepository();

            GetStacks();

            Console.WriteLine("\nInput the name of a stack you want to insert");
            Console.WriteLine("or type 0 to exit\n");
            string stack = Console.ReadLine();

            userRepository.InsertStackData(stack);
        }

        public string GetStackName(string interact)
        {
            var userRepository = new UserRepository();

            List<string> stackNames = userRepository.ViewStacksData();

            while (true)
            {
                Console.WriteLine($"\nInput the name of a stack you want to {interact}");
                Console.WriteLine("or type 0 to exit\n");
                string stack = Console.ReadLine();

                if (stackNames.Contains(stack))
                {
                    return stack;
                }
                if (stack == "0")
                {
                    MainMenu();
                    return "0";
                }
                Console.WriteLine("Please, type the name of one of the current stacks avaible.");
            }
        }

        private List<string> GetStacks()
        {
            var userRepository = new UserRepository();

            Console.WriteLine("Current Stacks");
            return userRepository.ViewStacksData();
        }

        private void FlashcardsMenu()
        {
            string stack = GetStackName("interact with");

            if (stack != "0")
                ManageFlashcards(stack);
        }

        private void DeleteFlashcards(string stackName)
        {
            var userRepository = new UserRepository();

            userRepository.ViewAllFlashcardsData(stackName);

            int id = GetFlashcardId(stackName, "delete");

            userRepository.DeleteFlashcardsData(id);
        }

        private void EditFlashcards(string stackName)
        {
            bool exit = false;
            var userRepository = new UserRepository();

            userRepository.ViewAllFlashcardsData(stackName);

            while (!exit)
            {
                int id = GetFlashcardId(stackName, "edit");
                string column = GetColumnName();
                string text = GetNewText();

                userRepository.UpdateFlashcardsData(id, column, text);

                exit = !ContinueEditing();
            }
        }

        public string GetNewText()
        {
            Console.WriteLine("Type the word or phrase you want to edit to:");
            return Console.ReadLine();
        }

        public bool ContinueEditing()
        {
            while (true)
            {
                Console.WriteLine("Do you want to continue editing? Type 'yes' to continue, or 'no' to exit:");
                string answer = Console.ReadLine().Trim().ToLower();

                if (answer == "yes")
                    return true;
                else if (answer == "no")
                    return false;
                else
                    Console.WriteLine("Invalid input. Please type 'yes' or 'no'.");
            }
        }

        public string GetColumnName()
        {
            while (true)
            {
                Console.WriteLine("Type the column you want to edit - front or back:");
                string column = Console.ReadLine().Trim().ToLower();

                if (column == "front" || column == "back")
                {
                    return column;
                }

                Console.WriteLine("Invalid input. Please type 'front or 'back'.");
            }
        }

        public int GetFlashcardId(string stackName, string updateDelete)
        {
            var UserRepository = new UserRepository();

            List<int> flashcardsId = UserRepository.GetFlashcardsId(stackName);

            int[] flashcardsConsoleId = new int[flashcardsId.Count];

            for (int i = 0; i < flashcardsConsoleId.Length; i++)
            {
                flashcardsConsoleId[i] = 1 + i;
            }

            while (true)
            {
                Console.WriteLine($"Type the Id of the card you want to {updateDelete}:");

                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    if (flashcardsConsoleId.Contains(id))
                        return flashcardsId[id - 1];
                }

                Console.WriteLine("Type a valid Id.");
            }
        }

        private void ViewFlashcardsFront(string stackName)
        {
            var userRepository = new UserRepository();

            userRepository.ViewFlashcardsFrontData(stackName);

            Console.WriteLine("\nType any key to exit\n");
            Console.ReadLine();

        }

        private void ViewAllFlashcards(string stackName)
        {
            var userRepository = new UserRepository();

            userRepository.ViewAllFlashcardsData(stackName);

            Console.WriteLine("\nType any key to exit\n");
            Console.ReadLine();
        }

        private void CreateFlashcards(string stackName)
        {
            var userRepository = new UserRepository();

            Console.WriteLine($"{stackName} stack\n");
            Console.WriteLine("Write the front of your flashcard: ");
            string front = Console.ReadLine();

            Console.WriteLine("Write the back of your flashcard: ");
            string back = Console.ReadLine();

            userRepository.InsertFlashcardsData(stackName, front, back);

        }

        private void ManageFlashcards(string stackName)
        {
            bool exit = false;

            UserRepository userRepository = new UserRepository();

            while (!exit)
            {
                Console.WriteLine($"Currently working on {stackName} stack\n");
                Console.WriteLine("Type to select an option:");
                Console.WriteLine("0 - Return to main menu");
                Console.WriteLine("1 - View front of all flashcards");
                Console.WriteLine("2 - View all flashcards");
                Console.WriteLine("3 - Create a flashcard in current stack");
                Console.WriteLine("4 - Edit a flashcard");
                Console.WriteLine("5 - Delete a flashcard");

                string op = Console.ReadLine();

                switch (op)
                {
                    case "0":
                        exit = true;
                        break;
                    case "1":
                        ViewFlashcardsFront(stackName);
                        break;
                    case "2":
                        ViewAllFlashcards(stackName);
                        break;
                    case "3":
                        CreateFlashcards(stackName);
                        break;
                    case "4":
                        EditFlashcards(stackName);
                        break;
                    case "5":
                        DeleteFlashcards(stackName);
                        break;
                    default:
                        Console.WriteLine("Type a valid option.");
                        break;
                }
            }
        }
    }
}