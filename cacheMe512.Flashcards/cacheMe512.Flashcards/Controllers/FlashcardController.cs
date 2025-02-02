using Dapper;
using cacheMe512.Flashcards.Models;
using cacheMe512.Flashcards.DTOs;

namespace cacheMe512.Flashcards.Controllers
{
    internal class FlashcardController
    {
        public IEnumerable<FlashcardDTO> GetFlashcardsByStackId(int stackId)
        {
            try
            {
                using var connection = Database.GetConnection();

                var flashcards = connection.Query<Flashcard>(
                    "SELECT Id, Question, Answer, StackId, Position FROM flashcards WHERE StackId = @StackId ORDER BY Position",
                    new { StackId = stackId }
                ).ToList();

                return flashcards.Select(fc => new FlashcardDTO(fc.Id, fc.Question, fc.Answer, fc.Position));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving flashcards: {ex.Message}");
                return Enumerable.Empty<FlashcardDTO>();
            }
        }

        public void InsertFlashcard(Flashcard flashcard)
        {
            try
            {
                using var connection = Database.GetConnection();
                using var transaction = connection.BeginTransaction();

                int nextPosition = connection.ExecuteScalar<int>(
                    "SELECT COALESCE(MAX(Position), 0) + 1 FROM flashcards WHERE StackId = @StackId",
                    new { flashcard.StackId },
                    transaction: transaction
                );

                connection.Execute(
                    "INSERT INTO flashcards (StackId, Question, Answer, Position) VALUES (@StackId, @Question, @Answer, @Position)",
                    new { flashcard.StackId, flashcard.Question, flashcard.Answer, Position = nextPosition },
                    transaction: transaction
                );

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

                var flashcard = connection.QueryFirstOrDefault<Flashcard>(
                    "SELECT StackId, Position FROM flashcards WHERE Id = @FlashcardId",
                    new { FlashcardId = flashcardId },
                    transaction: transaction
                );

                if (flashcard == null)
                {
                    Console.WriteLine("Flashcard not found.");
                    return;
                }

                connection.Execute(
                    "DELETE FROM flashcards WHERE Id = @FlashcardId",
                    new { FlashcardId = flashcardId },
                    transaction: transaction
                );

                connection.Execute(
                    "UPDATE flashcards SET Position = Position - 1 WHERE StackId = @StackId AND Position > @DeletedPosition",
                    new { StackId = flashcard.StackId, DeletedPosition = flashcard.Position },
                    transaction: transaction
                );

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting flashcard: {ex.Message}");
            }
        }
    }
}
