/*
This app allows you to create stacks of flashcards
to test your knowledge on a subject
*/
using System.Diagnostics;

namespace flashcard_app
{
    /// <summary>
    /// Responsible for the main app flow
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            DBController.InitialiseDB();
            AppFlow();
        }

        /// <summary>
        /// Handles the core loop of showing the user the main menu and accepting input
        /// </summary>
        static void AppFlow()
        {
            Console.Clear();

            bool quitApp = false;

            while (quitApp == false)
            {
                View.DisplayMainMenuOptions();

                string? command = Console.ReadLine();

                switch (command)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        quitApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        View.ShowStacksMainMenu(DBController.GetAllStacks());
                        HandleStackMainMenuSelection();
                        break;
                    case "2":
                        View.ShowFlashcardMainMenu(DBController.GetAllFlashcards());
                        HandleFlashcardMainMenuSelection();
                        break;
                    case "3":
                        StartStudy();
                        break;
                    case "4":
                        ViewStudySessionData();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                        break;
                }
            }
        }

        static void HandleStackMainMenuSelection()
        {
            int? stackID =
                UserInput.GetNumberInput($"Type a stack ID to edit one, or type 0 to make a new one: \n");

            if (stackID == 0)
            {
                // Make a new stack
                string? newStackName = UserInput.GetStringInput("Enter the name of the new stack:");
                int newStackID = DBController.AddNewStack(newStackName);

                // Allow user to add flashcards to this stack
                Console.Clear();
                View.ManageSingleStackMenu(newStackName);
                ManageSingleStackSelection(newStackID);
            }
            else
            {
                // Display all flashcards in this stack
                Console.Clear();
                List<FlashcardDTO> flashcardsInStack = DBController.GetFlashcardsByStackId(stackID);
                View.DisplayFlashcardsHorizontally(flashcardsInStack);
                View.ShowStackManageMenu(stackID, flashcardsInStack);
                HandleStackManageSelection(stackID, flashcardsInStack);
            }
        }

        private static void ManageSingleStackSelection(int stackID)
        {
            int? userStackOption = UserInput.GetNumberInput("Your choice: ");

            if (userStackOption == 1)  // Create a new flashcard for this stack
            {
                Console.Clear();
                string? frontText = UserInput.GetStringInput("Enter the front text for the flashcard:");
                string? backText = UserInput.GetStringInput("Enter the back text for the flashcard:");
                DBController.CreateNewFlashcard(frontText, backText, stackID);
            }
            else if (userStackOption == 2)   // View all flashcards for this stack
            {
                Console.Clear();
                View.DisplayFlashcardsHorizontally(DBController.GetFlashcardsByStackId(stackID));
            }
            else
            {
                Console.Clear();
            }
        }

        private static void HandleStackManageSelection(int? stackID, List<FlashcardDTO> flashcardsInStack)
        {
            int? userStackOption = UserInput.GetNumberInput("Your choice: ");

            if (userStackOption == 1)  // Change current stack
            {
                Console.Clear();
                View.ShowStacksMainMenu(DBController.GetAllStacks());
                return;
            }
            else if (userStackOption == 2)   // Create a flashcard in stack
            {
                Console.Clear();
                string? frontText = UserInput.GetStringInput("Enter the front text for the flashcard:");
                string? backText = UserInput.GetStringInput("Enter the back text for the flashcard:");
                DBController.CreateNewFlashcard(frontText, backText, stackID);
            }
            else if (userStackOption == 3)   // Edit a flashcard in stack
            {
                Console.Clear();
                View.DisplayFlashcardsHorizontally(DBController.GetFlashcardsByStackId(stackID));
                int flashcardID = UserInput.GetNumberInput("Which flashcard ID would you like to edit: ");
                string? frontText = UserInput.GetStringInput($"Enter the new front text for the flashcard: (previous text: {flashcardsInStack[flashcardID - 1].FrontText})");
                string? backText = UserInput.GetStringInput($"Enter the new back text for the flashcard: (previous text: {flashcardsInStack[flashcardID - 1].BackText})");
                DBController.UpdateFlashcard(flashcardsInStack[flashcardID - 1].FlashcardID, frontText, backText);
            }
            else if (userStackOption == 4)   // Delete a flashcard in stack
            {
                View.DisplayFlashcardsHorizontally(DBController.GetFlashcardsByStackId(stackID));
                int flashcardID = UserInput.GetNumberInput("Which flashcard ID would you like to delete: ");
                DBController.DeleteFlashcard(flashcardsInStack[flashcardID - 1].FlashcardID);
            }
            else
            {
                Console.Clear();
                return;   // 0 for main menu or anything else was typed
            }
        }

        static void HandleFlashcardMainMenuSelection()
        {
            int? userSelection = UserInput.GetNumberInput("");
            List<FlashcardDTO> flashcards = DBController.GetAllFlashcards();

            if (userSelection == 1)   // Edit a flashcard
            {
                View.DisplayFlashcardsHorizontally(DBController.GetAllFlashcards());
                int flashcardID = UserInput.GetNumberInput("Which flashcard ID would you like to edit: ");
                string? frontText = UserInput.GetStringInput($"Enter the new front text for the flashcard: (previous text: {flashcards[flashcardID - 1].FrontText})");
                string? backText = UserInput.GetStringInput($"Enter the new back text for the flashcard: (previous text: {flashcards[flashcardID - 1].BackText})");
                DBController.UpdateFlashcard(flashcards[flashcardID - 1].FlashcardID, frontText, backText);
            }
            else if (userSelection == 2)   // Delete a flashcard
            {
                View.DisplayFlashcardsHorizontally(DBController.GetAllFlashcards());
                int flashcardID = UserInput.GetNumberInput("Which flashcard ID would you like to delete: ");
                DBController.DeleteFlashcard(flashcards[flashcardID - 1].FlashcardID);
            }
            else
            {
                Console.Clear();
                return;   // 0 for main menu or anything else was typed
            }
        }

        private static void StartStudy()
        {
            Console.Clear();
            int numberCorrect = 0;

            if (DBController.GetAllStacks().Count == 0)
            {
                Console.WriteLine("There are currently no stacks, press any key to return to main menu.");
                Console.ReadKey();
                return;
            }

            // Choose a stack
            int stackID = UserInput.GetNumberInput("Which stack would you like to study? Enter ID:");

            List<FlashcardDTO> flashcards = DBController.GetFlashcardsByStackId(stackID);

            if (flashcards.Count == 0 || flashcards == null)
            {
                Console.WriteLine("There are currently no flashcards in this stack, press any key to return to main menu.");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            // Randomise the flashcards
            Random rng = new Random();
            List<FlashcardDTO> shuffledCards = flashcards.OrderBy(x => rng.Next()).ToList();

            foreach (FlashcardDTO flashcard in shuffledCards)
            {
                Console.Clear();
                View.DisplaySingleFlashcard(flashcard);
                string? backTextGuess = UserInput.GetStringInput("What is the back text?");

                if (backTextGuess != null && backTextGuess.ToLower() == flashcard.BackText.ToLower())
                {
                    Console.WriteLine($"Correct! Press any key to continue.");
                    Console.ReadKey();
                    numberCorrect++;
                }
                else
                {
                    Console.WriteLine($"Incorrect. Press any key to continue.");
                    Console.ReadKey();
                }
            }

            // When all done, show results , on key press return to main menu
            DBController.CreateStudySession(stackID, numberCorrect, flashcards.Count);
            Console.WriteLine($"\nYou got {numberCorrect}/{flashcards.Count} correct. Press any key to return to main menu.");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        private static void ViewStudySessionData()
        {
            Console.Clear();
            View.ShowStudySessionData();
            Console.WriteLine("Press any key to return to the main menu.");
            Console.ReadKey();
            Console.Clear();
        }
    }
}