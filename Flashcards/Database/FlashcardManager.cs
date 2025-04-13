using Flashcards.Object_Classes;
using Flashcards.Validation;

namespace Flashcards.Database
{
    public static class FlashcardManager
    {
        public static void CreateFlashcard()
        {
            StackManager.ViewAllFlashcardStacks();

            int stackID = InputValidation.GetQuantity("Enter the ID of the Flashcard Stack you want to add this card to, or type 0 to return to the Main Menu.");

            string? question = null;
            string? answer = null;

            Console.WriteLine("Please provide a question for your new Flashcard.");
            question = Console.ReadLine();
            while (InputValidation.IsValidString(question) == false)
            {
                Console.WriteLine("Please provide a question for your new Flashcard.");
                question = Console.ReadLine();
            }

            Console.WriteLine("Please provide an answer for your new Flashcard.");
            answer = Console.ReadLine();
            while (InputValidation.IsValidString(answer) == false)
            {
                Console.WriteLine("Please provide an answer for your new Flashcard.");
                answer = Console.ReadLine();
            }

            DatabaseManager.CreateFlashcard(question, answer, stackID);
            Console.WriteLine($"\nFlashcard successfully created.");
        }

        public static void UpdateFlashcard()
        {
            if (!DatabaseManager.DoWeHaveFlashCards())
            {
                Console.WriteLine("No flashcards found. Please create flashcards first.");
                return;
            }
            ViewAllFlashcards();

            int flashcardID = InputValidation.GetQuantity("Enter the ID of the Flashcard you wish to update, or type 0 to return to the Main Menu.");

            while (flashcardID != 0 && DatabaseValidator.DoesValueExist(System.Configuration.ConfigurationManager.AppSettings.Get("FlashcardTable"),
                "FlashcardID", flashcardID) == false)
            {
                Console.WriteLine("Please provide a valid Flashcard ID.");
                flashcardID = InputValidation.GetQuantity("Enter the ID of the Flashcard you wish to update, or type 0 to return to the Main Menu.");
            }

            string? question = null;
            string? answer = null;

            Console.WriteLine("Please provide a new question for your Flashcard.");
            question = Console.ReadLine();
            while (InputValidation.IsValidString(question) == false)
            {
                Console.WriteLine("Please provide a question for your new Flashcard.");
                question = Console.ReadLine();
            }

            Console.WriteLine("Please provide a new answer for your Flashcard.");
            answer = Console.ReadLine();
            while (InputValidation.IsValidString(answer) == false)
            {
                Console.WriteLine("Please provide an answer for your new Flashcard.");
                answer = Console.ReadLine();
            }

            DatabaseManager.UpdateFlashcard(question, answer, flashcardID);
            Console.WriteLine($"\nFlashcard with ID {flashcardID} successfully updated.");
        }

        public static void DeleteFlashcard()
        {
            if (!DatabaseManager.DoWeHaveFlashCards())
            {
                Console.WriteLine("No flashcards found. Please create flashcards first.");
                return;
            }
            ViewAllFlashcards();

            int flashcardID = InputValidation.GetQuantity("Enter the ID of the Flashcard you wish to delete, or type 0 to return to the Main Menu.");

            while (flashcardID != 0 && DatabaseValidator.DoesValueExist(System.Configuration.ConfigurationManager.AppSettings.Get("FlashcardTable"),
                "FlashcardID", flashcardID) == false)
            {
                Console.WriteLine("Please provide a valid Flashcard ID.");
                flashcardID = InputValidation.GetQuantity("Enter the ID of the Flashcard you wish to delete, or type 0 to return to the Main Menu.");
            }
            DatabaseManager.DeleteFlashcard(flashcardID);
            Console.WriteLine($"\nFlashcard with ID {flashcardID} successfully deleted.");
        }

        public static void ViewFlashcards()
        {
            if (!DatabaseManager.DoWeHaveFlashCards())
            {
                Console.WriteLine("No flashcards found. Please create flashcards first.");
                return;
            }
            int flashcardID = InputValidation.GetQuantity("Enter the ID of the Flashcard you wish to view, type -1 to view all Flashcards \n or type 0 to return to the Main Menu.");
            while (flashcardID != 0 && flashcardID != -1 && DatabaseValidator.DoesValueExist(System.Configuration.ConfigurationManager.AppSettings.Get("FlashcardTable"),
                "FlashcardID", flashcardID) == false)
            {
                Console.WriteLine("Please provide a valid Flashcard ID.");
                flashcardID = InputValidation.GetQuantity("Enter the ID of the Flashcard you wish to view, type -1 to view all Flashcards \n or type 0 to return to the Main Menu.");
            }
            List<FlashcardDTO> flashcards = DatabaseManager.GetFlashcards(flashcardID);
            Console.WriteLine("----------------------------------------------\n");
            foreach (var card in flashcards)
            {
                Console.WriteLine($"ID: {card.FlashcardId} | Question: {card.Question} | Answer: {card.Answer}");
            }
            Console.WriteLine("----------------------------------------------\n");
        }

        public static void ViewAllFlashcards()
        {
            List<FlashcardDTO> flashcards = DatabaseManager.GetFlashcards();

            Console.WriteLine("----------------------------------------------\n");
            foreach (var card in flashcards)
            {
                Console.WriteLine($"ID: {card.FlashcardId} | Question: {card.Question} | Answer: {card.Answer}");
            }
        }
    }
}
