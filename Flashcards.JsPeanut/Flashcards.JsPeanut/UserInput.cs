using TestingArea;

namespace Flashcards.JsPeanut
{
    class UserInput
    {
        public static void GetUserInput()
        {
            bool exit = false;
            while (exit == false)
            {
                string[] options = { "Create a new stack", "Add a flashcard", "Get into the study zone", "Display stacks", "Display flashcards", "Display my study sessions", "Remove stacks", "Remove flashcards", "Update stacks", "Update flashcards", "Total of sessions per month", "Average score per month"};
                List<ConsoleKey> KeysToUse = new()
                {
                    ConsoleKey.UpArrow,
                    ConsoleKey.DownArrow,
                };
                switch (Menu.ShowMenu(KeysToUse, "Welcome to Flashcards app!", "\nUse UP ARROW  and DOWN ARROW to navigate and press \u001b[32mEnter/Return\u001b[0m to select:\n", options))
                {
                    case 0:
                        Console.Clear();
                        CodingController.CreateStack();
                        break;
                    case 1:
                        Console.Clear();
                        CodingController.CreateFlashcard();
                        break;
                    case 2:
                        Console.Clear();
                        CodingController.StudyZone();
                        break;
                    case 3:
                        Console.Clear();
                        CodingController.RetrieveStacks("display");
                        break;
                    case 4:
                        Console.Clear();
                        CodingController.RetrieveFlashcards("display");
                        break;
                    case 5:
                        Console.Clear();
                        CodingController.RetrieveStudySessions("display");
                        break;
                    case 6:
                        Console.Clear();
                        CodingController.RemoveStack();
                        break;
                    case 7:
                        Console.Clear();
                        CodingController.RemoveFlashcard();
                        break;
                    case 8:
                        Console.Clear();
                        CodingController.UpdateStack();
                        break;
                    case 9:
                        Console.Clear();
                        CodingController.UpdateFlashcard();
                        break;
                    case 10:
                        Console.Clear();
                        CodingController.MonthlySessionsReport();
                        break;
                    case 11:
                        Console.Clear();
                        CodingController.AverageScorePerMonth();
                        break;
                }
            }
        }

        public static string GetStackName()
        {
            Console.WriteLine("Type the name of the stack. Type M if you want to return to the main menu!");
            string stackName = Console.ReadLine();

            if (stackName == "M") GetUserInput();

            while (Validation.ValidateStackOrFlashcard(stackName) == "invalid")
            {
                Console.WriteLine("Invalid text. Try again or type M to return to the main menu.");
                if (stackName == "M") GetUserInput();
                stackName = Console.ReadLine();
            }

            return stackName;
        }

        public static int GetToWhichStackItCorresponds()
        {
            CodingController.RetrieveStacks("retrieve");
            if (Validation.CheckIfThereAreStacks() == "zero")
            {
                Console.WriteLine("You got to have at least one stack before trying to add a flashcard!");
                GetUserInput();
            }
            Console.WriteLine("Type to which stack your flashcard belongs. Type M if you want to return to the main menu!");
            CodingController.RetrieveStacks("display");
            
            string stackId = Console.ReadLine();

            if (stackId == "M") GetUserInput();

            while (Validation.ValidateNumber(stackId) == "invalid")
            {
                Console.WriteLine("Invalid number. Try again or type M to return to the main menu.");
                if (stackId == "M") GetUserInput();
                stackId = Console.ReadLine();
            }

            int stackId_ = Convert.ToInt32(stackId);

            return stackId_;
        }
        public static string GetFlashcardQuestion()
        {
            Console.WriteLine("Type the name of the question for your flashcard. Type M if you want to return to the main menu!");

            string flashcardQ = Console.ReadLine();

            if (flashcardQ == "M") GetUserInput();

            while (Validation.ValidateStackOrFlashcard(flashcardQ) == "invalid")
            {
                Console.WriteLine("Invalid. Try again. Type M if you want to return to the main menu!");

                flashcardQ = Console.ReadLine();

                if (flashcardQ == "M") GetUserInput();
            }

            return flashcardQ;
        }
        public static string GetFlashcardAnswer()
        {
            Console.WriteLine("Type the name of the answer for your flashcard. Type M if you want to return to the main menu!");
            string flashcardA = Console.ReadLine();

            if (flashcardA == "M") GetUserInput();

            while (Validation.ValidateStackOrFlashcard(flashcardA) == "invalid")
            {
                Console.WriteLine("Invalid string. Try again or type M to return to the main menu.");
                if (flashcardA == "M") GetUserInput();
                flashcardA = Console.ReadLine();
            }

            return flashcardA;
        }

        public static int GetRequiredId(string message)
        {
            Console.WriteLine(message);
            string stackId = Console.ReadLine();

            if (stackId == "M") GetUserInput();

            while (Validation.ValidateNumber(stackId) == "invalid")
            {
                Console.WriteLine("Invalid number. Try again or type M to return to the main menu.");
                if (stackId == "M") GetUserInput();
                stackId = Console.ReadLine();
            }

            int stackId_ = Convert.ToInt32(stackId);

            return stackId_;
        }

        public static string GetFrontOfTheCard(string message)
        {
            Console.WriteLine(message);

            string frontOfTheCard = Console.ReadLine();

            while (Validation.ValidateStackOrFlashcard(frontOfTheCard) == "invalid")
            {
                Console.WriteLine("Invalid string. Try again or type M to return to the main menu.");
                if (frontOfTheCard == "M") GetUserInput();
                frontOfTheCard = Console.ReadLine();
            }

            if (frontOfTheCard == "M") GetUserInput();

            return frontOfTheCard;
        }
    }
}
