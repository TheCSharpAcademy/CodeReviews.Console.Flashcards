using Dapper;
using Flashcards.Model;
using Flashcards.View;
using Microsoft.Data.SqlClient;

namespace Flashcards.Controller
{
    public class FlashcardsController
    {
        public static void AddFlashcard()
        {
            var (stack, index) = Display.PrintStackSelectionMenu("Add Flashcard", "Select the stack you want to add a flashcard to...");

            int stackId = stack.Id;

            Display.PrintAllFlashcardsForStack("Add Flashcard", stackId);

            string question = UI.PromptForAlphaNumericInput("\nEnter the flashcard's question: ");
            string answer = UI.PromptForAlphaNumericInput("Enter the flashcard's answer: ");

            string insertQuery = @"
                    INSERT INTO Flashcards (StackID, Question, Answer)
                    VALUES (@stackId, @question, @answer)";

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                int rowsAffected = connection.Execute(insertQuery, new { stackId, question, answer });

                if (rowsAffected > 0)
                {
                    Display.PrintAllFlashcardsForStack("Add Flashcard", stackId);
                    Console.WriteLine("\nFlashcard added successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to add flashcard. Please try again.");
                }
            }
        }

        public static void EditFlashcard()
        {
            var (stack, stackIndex) = Display.PrintStackSelectionMenu("Edit Flashcard", "Select the stack of the flashcard you want to edit...");

            int stackId = stack.Id;

            var (flashcard, flashcardIndex) = Display.PrintFlashcardSelectionMenu("Edit Flashcard", "Select the flashcard you want to edit...", stackId);

            int flashcardId = flashcard.FlashcardId;

            int flashcardIndexPlusOne = flashcardIndex + 1;

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                string flashcardQuery = "SELECT * FROM Flashcards WHERE flashcardId = @flashcardId";
                var flashcardData = connection.QuerySingleOrDefault(flashcardQuery, new {flashcardId});

                Display.PrintAllFlashcardsForStack("Edit Flashcard", stackId);

                Console.WriteLine($"\nSelected flashcard ID: {flashcardIndexPlusOne}");
                Console.WriteLine($"Question: {flashcardData.Question}");
                Console.WriteLine($"Answer: {flashcardData.Answer}");

                string newQuestion = UI.PromptForAlphaNumericInput($"\nEnter new question (leave blank to keep current): ", true);

                if (string.IsNullOrEmpty(newQuestion))
                {
                    newQuestion = flashcardData.Question;
                }

                string newAnswer = UI.PromptForAlphaNumericInput($"\nEnter new answer (leave blank to keep current): ", true);

                if (string.IsNullOrEmpty(newAnswer))
                {
                    newAnswer = flashcardData.Answer!;
                }

                if (newQuestion == flashcardData.Question && newAnswer == flashcardData.Answer)
                {
                    Console.WriteLine("\nNo changes were made.");
                    return;
                }

                string updateFlashcardQuery = "UPDATE Flashcards SET Question = @newQuestion, Answer = @newAnswer WHERE FlashcardId = @flashcardId";

                int rowsAffected = connection.Execute(updateFlashcardQuery, new {flashcardId, newQuestion, newAnswer});

                if (rowsAffected > 0)
                {
                    Display.PrintAllFlashcardsForStack("Edit Flashcard", stackId);
                    Console.WriteLine("\nFlashcard updated successfully!");
                }
                else
                {
                    Console.WriteLine("\nFailed to update flashcard.");
                }
            }
        }

        public static void DeleteFlashcard()
        {
            var (stack, stackIndex) = Display.PrintStackSelectionMenu("Delete Flashcard", "Select the stack of the flashcard you want to delete...");

            int stackId = stack.Id;


            var (flashcard, flashcardIndex) = Display.PrintFlashcardSelectionMenu("Delete Flashcard", "Select the flashcard you want to delete...", stackId);

            int flashcardId = flashcard.FlashcardId;

            int flashcardIndexPlusOne = flashcardIndex + 1;

            Display.PrintAllFlashcardsForStack("Delete Flashcard", stackId);

            if (UI.PromptForDeleteConfirmation(flashcardIndexPlusOne, "flashcard") == "n")
            {
                return;
            }

            string deleteQuery = @"
                    DELETE FROM Flashcards
                    WHERE FlashcardId = @flashcardId";

            using (var connection = new SqlConnection(DatabaseUtility.GetConnectionString()))
            {
                connection.Open();

                int rowsAffected = connection.Execute(deleteQuery, new {flashcardId});

                if (rowsAffected > 0)
                {
                    Display.PrintAllFlashcardsForStack("Delete Flashcard", stackId);
                    Console.WriteLine("\nFlashcard deleted successfully!");
                }
                else
                {
                    Console.WriteLine("\nNo flashcard found with that ID. Deletion failed.");
                }
            }
        }
    }
}
