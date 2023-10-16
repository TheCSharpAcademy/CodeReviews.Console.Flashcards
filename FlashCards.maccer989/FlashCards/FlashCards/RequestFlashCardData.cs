using CodingTrackerConsoleUI;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;

namespace FlashCardsUI
{
    public static class RequestFlashCardData
    {
        public static void UpdateFlashCard(SqlFlashCardsCrud sql)
        {
            Console.Clear();
            ReadAllFlashCards(sql);
            int stackName = GetNumberInput("\nPlease type the name of the Stack you want to update. Type 0 to return to Main Menu.");
            int cardId = GetNumberInput("\nPlease type the name of the Flashcard you want to update. Type 0 to return to Main Menu.");
            (stackName, cardId) = CheckValidRecord(sql, stackName, cardId);
            Console.WriteLine("Please enter new card question:");
            string cardQuestion = Console.ReadLine();
            Console.WriteLine("Please enter new card answer:");
            string cardAnswer = Console.ReadLine();
            DateTime lastUpdated = DateTime.Now;
            sql.UpdateFlashCard(stackName, cardId, cardQuestion, cardAnswer);
        }
        public static void RemoveFlashCard(SqlFlashCardsCrud sql)
        {
            Console.Clear();
            ReadAllFlashCards(sql);
            int stackName = GetNumberInput("\nPlease type the Stack name of the Flashcard you want to delete. Type 0 to return to Main Menu.");
            int cardId = GetNumberInput("\nPlease type the Flashcard you want to delete. Type 0 to return to Main Menu.");
            (stackName, cardId) = CheckValidRecord(sql, stackName, cardId);
            sql.RemoveFlashCard(stackName, cardId);
        }
        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();
            if (numberInput == "0")
            {
                Console.WriteLine("Incorrect choice returning you to Main Menu");
                UserInterface.GetMainMenu();
            }
            int finalInput = Validation.CheckValidNumber(numberInput);
            return finalInput;
        }
        static (int, int) CheckValidRecord(SqlFlashCardsCrud sql, int stackName, int flashCardId)
        {
            bool recordExists = sql.CheckRecordExists(stackName, flashCardId);
            while (!recordExists)
            {
                stackName = GetNumberInput("\nPlease type the Stack name of the Flashcard. Type 0 to return to Main Menu.");
                flashCardId = GetNumberInput("\nPlease type the name of the Flashcard. Type 0 to return to Main Menu.");
                recordExists = sql.CheckRecordExists(stackName, flashCardId);
            }
            return (stackName, flashCardId);
        }
        static (int, int) CheckDuplicateRecord(SqlFlashCardsCrud sql, int stackName, int flashCardId)
        {
            bool recordExists = sql.CheckRecordExists(stackName, flashCardId);
            while (recordExists)
            {
                Console.Clear();
                ReadAllFlashCards(sql);
                stackName = GetNumberInput("\nPlease type the name of the Stack name of the Flashcard you want to delete. Enter a valid number between 1 and 9 or type 0 to return to Main Menu.");
                flashCardId = GetNumberInput("\nPlease type the name of the Flashcard you want to delete. Enter a valid number between 1 and 9 or type 0 to return to Main Menu.");
                recordExists = sql.CheckRecordExists(stackName, flashCardId);
            }
            return (stackName, flashCardId);
        }
        public static void CreateNewFlashCard(SqlFlashCardsCrud sql)
        {
            int stackName;
            int flashCardId;
            string cardQuestion;
            string cardAnswer;
            stackName = GetNumberInput("\nPlease type the name of the Stack you want to add. Enter a valid number between 1 and 9 or type 0 to return to Main Menu.");
            bool stackExists = sql.CheckStackExists(stackName);
            while (!stackExists)
            {
                stackName = GetNumberInput("\nPlease type the name of the Stack you want to add. Enter a valid number between 1 and 9 or type 0 to return to Main Menu.");
                stackExists = sql.CheckStackExists(stackName);
            }
            flashCardId = GetNumberInput("\nPlease type the name of the Flashcard you want to add. Enter a valid number between 1 and 9 or type 0 to return to Main Menu.");
            bool recordExists = sql.CheckRecordExists(stackName, flashCardId);
            while (recordExists)
            {
                stackName = GetNumberInput("\nPlease type the name of the Stack you want to add. Enter a valid number between 1 and 9 or type 0 to return to Main Menu.");
                flashCardId = GetNumberInput("\nPlease type the name of the Flashcard you want to add. Enter a valid number between 1 and 9 or type 0 to return to Main Menu.");
                recordExists = sql.CheckRecordExists(stackName, flashCardId);
            }
            Console.WriteLine("Please enter a card question:");
            cardQuestion = Console.ReadLine();
            Console.WriteLine("Please enter a card answer:");
            cardAnswer = Console.ReadLine();
            FlashCardsModel card = new FlashCardsModel
            {
                StackName = stackName,
                FlashCardId = flashCardId,
                Question = cardQuestion,
                Answer = cardAnswer
            };
            sql.CreateFlashCard(card);
        }
        public static void ReadAllFlashCards(SqlFlashCardsCrud sql)
        {
            var rows = sql.GetAllFlashCardsByStackId();
            TableVisualisation.ShowTable(rows);
        }
    }
}
