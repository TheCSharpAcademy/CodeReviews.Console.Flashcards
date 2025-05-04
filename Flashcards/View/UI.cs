using Flashcards.Model;

namespace Flashcards.View
{
    internal class UI
    {
        public static void ReturnToMainMenu()
        {
            Console.Write("\nPress any key to return to the Main Menu...");
            Console.ReadKey();
        }

        public static string PromptForAlphaNumericInput(string message, bool forEdit = false, bool forStackName = false)
        {
            string? input;
            bool isValidInput = false;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (!forEdit)
                {
                    if (Validation.ValidateAlphaNumericInput(input) == false)
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    else if (forStackName)
                    {
                        if (Validation.ValidateAlphaNumericInput(input, false, true) == false)
                        {
                            Console.WriteLine("Invalid input. A stack with that name already exists.");
                        }
                        else
                        {
                            isValidInput = true;
                        }
                    }
                    else
                    {
                        isValidInput = true;
                    }
                }
                else
                    if (Validation.ValidateAlphaNumericInput(input, true) == false)
                {
                    Console.WriteLine("Invalid input.");
                }
                else
                {
                    isValidInput = true;
                }
            } while (isValidInput == false);

            return input;
        }

        public static string PromptForReportYear(string message)
        {
            string? yearInput;
            bool isValidInput = false;
            do
            {
                Console.Write(message);
                yearInput = Console.ReadLine();
                if (Validation.ValidateYearInput(yearInput))
                {
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine($"Invalid input. Please enter a valid year between 2000 and {DateTime.Now.Year} in yyyy format.");
                }
            } while (isValidInput == false);

            return yearInput;
        }

        public static int PromptForId(string message, string tableName)
        {
            string? input;
            bool isValidInput = false;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (Validation.ValidateNumericInput(input!) == false)
                {
                    Console.WriteLine("Invalid input.");
                }
                else if (DatabaseUtility.CheckIfIdExists(DatabaseUtility.GetConnectionString(), tableName, Convert.ToInt32(input)))
                {
                    isValidInput = true;
                }
                else
                {
                    Console.WriteLine("Record not found. Please enter a valid ID.");
                }
            } while (isValidInput == false);

            return Convert.ToInt32(input);
        }

        public static int PromptForNumberOfFlashcards(string message, int stackId)
        {
            string? input;
            int numberOfFlashcardsToStudy;
            bool isValidInput = false;
            var flashcardsRepo = new FlashcardsRepository(DatabaseUtility.GetConnectionString());
            int numberOfFlashcardsInStack = flashcardsRepo.GetAllFlashcardsForStack(stackId).Count;
            do
            {
                Console.Write(message);
                input = Console.ReadLine();
                if (Validation.ValidateNumericInput(input!) == false)
                {
                    Console.WriteLine("Invalid input.");
                }
                else if (Validation.ValidateNumericInput(input!) == true)
                {
                    numberOfFlashcardsToStudy = Convert.ToInt32(input);

                    if (numberOfFlashcardsInStack < numberOfFlashcardsToStudy)
                    {
                        Console.WriteLine($"This stack only has {numberOfFlashcardsInStack} flashcards.");
                    }
                    else
                    {
                        isValidInput = true;
                    }
                }

            } while (isValidInput == false);

            return Convert.ToInt32(input);
        }

        public static string PromptForDeleteConfirmation(int indexPlusOne, string recordType)
        {
            string? confirmation;
            bool isValidConfirmation = false;
            do
            {
                Console.Write($"\nAre you sure you want to delete the {recordType} with ID {indexPlusOne}? (y/n): ");
                confirmation = Console.ReadLine();
                isValidConfirmation = Validation.ValidateDeleteConfirmation(confirmation);
                if (isValidConfirmation)
                {
                    if (confirmation == "n")
                    {
                        Console.WriteLine("Deletion canceled.");
                        return "n";
                    }
                    else
                    {
                        return "y";
                    }
                }
                else
                {
                    Console.WriteLine("Invalid response.\n");
                }
            } while (isValidConfirmation == false);

            return "";
        }
    }
}
