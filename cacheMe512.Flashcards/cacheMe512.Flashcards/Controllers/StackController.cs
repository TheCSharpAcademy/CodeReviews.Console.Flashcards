using cacheMe512.Flashcards.Models;
using cacheMe512.Flashcards.DTOs;
using Dapper;

namespace cacheMe512.Flashcards.Controllers
{
    internal class StackController
    {
        private readonly FlashcardController _flashcardController = new();

        public IEnumerable<StackDto> GetAllStacks()
        {
            try
            {
                using var connection = Database.GetConnection();

                var stacks = connection.Query<Stack>(
                    "SELECT Id, Name, Position, CreatedDate FROM stacks ORDER BY Position"
                ).ToList();

                return stacks.Select(s =>
                {
                    var flashcards = _flashcardController.GetFlashcardsByStackId(s.Id).ToList();
                    return new StackDto(s.Id, s.Name, flashcards, s.Position);
                });
            }
            catch (Exception ex)
            {
                Utilities.DisplayMessage($"Error retrieving stacks: {ex.Message}", "red");
                return Enumerable.Empty<StackDto>();
            }
        }

        public void InsertStack(Stack stack)
        {
            try
            {
                using var connection = Database.GetConnection();
                using var transaction = connection.BeginTransaction();

                int nextPosition = connection.ExecuteScalar<int>(
                    "SELECT COALESCE(MAX(Position), 0) + 1 FROM stacks",
                    transaction: transaction
                );

                connection.Execute(
                    "INSERT INTO stacks (Name, Position, CreatedDate) VALUES (@Name, @Position, @CreatedDate)",
                    new { Name = stack.Name, Position = nextPosition, CreatedDate = stack.CreatedDate },
                    transaction: transaction
                );

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Utilities.DisplayMessage($"Error inserting stack: {ex.Message}", "red");
            }
        }

        public bool DeleteStack(int stackId)
        {
            try
            {
                using var connection = Database.GetConnection();
                using var transaction = connection.BeginTransaction();

                var stack = connection.QueryFirstOrDefault<Stack>(
                    "SELECT Position FROM stacks WHERE Id = @StackId",
                    new { StackId = stackId },
                    transaction: transaction
                );

                if (stack == null)
                {
                    Console.WriteLine("Stack not found.");
                    return false;
                }

                connection.Execute(
                    "DELETE FROM stacks WHERE Id = @StackId",
                    new { StackId = stackId },
                    transaction: transaction
                );

                connection.Execute(
                    "UPDATE stacks SET Position = Position - 1 WHERE Position > @DeletedPosition",
                    new { DeletedPosition = stack.Position },
                    transaction: transaction
                );

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting stack: {ex.Message}");
                return false;
            }
        }
    }
}
