using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Flashcards.JsPeanut
{
    class UserInput
    {
        public static void GetUserInput()
        {
            bool exit = false;
            Console.Clear();
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Welcome to Flashcards app!");
            Console.ResetColor();
            Console.WriteLine("\nUse UP ARROW  and DOWN ARROW to navigate and press \u001b[32mEnter/Return\u001b[0m to select:");
            (int left, int top) = Console.GetCursorPosition();
            var option = 1;
            var decorator = $"✅ \u001b[32m";
            ConsoleKeyInfo key;
            bool isSelected = false;

            while (!isSelected)
            {
                Console.SetCursorPosition(left, top);

                Console.WriteLine($"{(option == 1 ? decorator : "  ")}Create a new stack\u001b[0m");
                Console.WriteLine($"{(option == 2 ? decorator : "  ")}Add a flashcard\u001b[0m");
                Console.WriteLine($"{(option == 3 ? decorator : "  ")}Get into the study zone\u001b[0m");
                Console.WriteLine($"{(option == 4 ? decorator : "  ")}Display stacks\u001b[0m");
                Console.WriteLine($"{(option == 5 ? decorator : "  ")}Display flashcards\u001b[0m");

                key = Console.ReadKey(false);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        option = option == 1 ? 5 : option - 1;
                        break;

                    case ConsoleKey.DownArrow:
                        option = option == 5 ? 1 : option + 1;
                        break;

                    case ConsoleKey.Enter:
                        isSelected = true;
                        break;
                }
            }

            Console.WriteLine($"\n{decorator}You selected Option {option}");
            Console.ResetColor();
            while (exit == false)
            {
                switch (option)
                {
                    case 1:
                        Console.Clear();
                        CodingController.CreateStack();
                        break;
                    case 2:
                        CodingController.CreateFlashcard();
                        break;
                    case 3:
                        CodingController.StudyZone();
                        break;
                    case 4:
                        CodingController.RetrieveStacks("display");
                        break;
                    case 5:
                        CodingController.PopulateFlashcardsList();
                        break;
                }
            }
        }

        public static string GetStackName()
        {
            Console.WriteLine("Type the name of the stack. Type M if you want to return to the main menu!");
            string stackName = Console.ReadLine();

            if (stackName == "M") GetUserInput();

            if (Validation.ValidateStackOrFlashcard(stackName) == "invalid")
            {
                Console.WriteLine("Invalid");
                GetStackName();
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

            if (Validation.ValidateNumber(stackId) == "invalid")
            {
                Console.WriteLine("Invalid number. Try again");
                GetToWhichStackItCorresponds();
            }

            int stackId_ = Convert.ToInt32(stackId);

            return stackId_;
        }
        public static string GetFlashcardQuestion()
        {
            Console.WriteLine("Type the name of the question for your flashcard. Type M if you want to return to the main menu!");

            string flashcardQ = Console.ReadLine();

            if (flashcardQ == "M") GetUserInput();

            if (Validation.ValidateStackOrFlashcard(flashcardQ) == "invalid")
            {
                GetFlashcardQuestion();
            }

            return flashcardQ;
        }
        public static string GetFlashcardAnswer()
        {
            Console.WriteLine("Type the name of the answer for your flashcard. Type M if you want to return to the main menu!");
            string flashcardA = Console.ReadLine();

            if (flashcardA == "M") GetUserInput();

            if (Validation.ValidateStackOrFlashcard(flashcardA) == "invalid")
            {
                GetFlashcardAnswer();
            }

            return flashcardA;
        }

        public static int GetStackIdForStudy()
        {
            Console.WriteLine("Type the number of the stack you want to study. Type M if you want to return to the main menu!");
            string stackId = Console.ReadLine();

            if (stackId == "M") GetUserInput();

            if (Validation.ValidateNumber(stackId) == "invalid")
            {
                GetStackIdForStudy();
            }

            int stackId_ = Convert.ToInt32(stackId);

            return stackId_;
        }
    }
}
