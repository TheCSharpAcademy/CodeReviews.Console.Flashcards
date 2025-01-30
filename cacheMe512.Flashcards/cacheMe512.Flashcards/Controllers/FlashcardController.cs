using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using cacheMe512.Flashcards.Models;

namespace cacheMe512.Flashcards.Controllers
{
    internal class FlashcardController
    {
        public IEnumerable<Flashcard> GetFlashcardsByStackId(int stackId)
        {
            try
            {
                using var connection = Database.GetConnection();

                var flashcards = connection.Query<Flashcard>(
                    "SELECT Id, Question, Answer, StackId FROM flashcards WHERE StackId = @StackId",
                    new { StackId = stackId }).ToList();

                return flashcards;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving flashcards: {ex.Message}");
                return Enumerable.Empty<Flashcard>();
            }
        }

        public void InsertFlashcard(Flashcard flashcard)
        {
            try
            {
                using var connection = Database.GetConnection();
                using var transaction = connection.BeginTransaction();

                connection.Execute(
                    "INSERT INTO flashcards (StackId, Question, Answer) VALUES (@StackId, @Question, @Answer)",
                    new { flashcard.StackId, flashcard.Question, flashcard.Answer },
                    transaction: transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting flashcard: {ex.Message}");
            }
        }

        public void DeleteFlashcard(int flashcardId)
        {
            try
            {
                using var connection = Database.GetConnection();
                using var transaction = connection.BeginTransaction();

                connection.Execute(
                    "DELETE FROM flashcards WHERE Id = @FlashcardId",
                    new { FlashcardId = flashcardId },
                    transaction: transaction);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting flashcard: {ex.Message}");
            }
        }
    }
}

