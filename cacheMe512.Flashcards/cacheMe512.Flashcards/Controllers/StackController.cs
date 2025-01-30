using cacheMe512.Flashcards.Models;
using Dapper;

namespace cacheMe512.Flashcards.Controllers;

internal class StackController
{
    public IEnumerable<Stack> GetAllStacks()
    {
        try
        {
            using var connection = Database.GetConnection();

            var stacks = connection.Query<Stack>(
                @"SELECT Id, 
                             Name, 
                             CreatedDate
                      FROM stacks").ToList();

            return stacks;
        }
        catch (Exception ex)
        {
            Utilities.DisplayMessage($"Error retrieving stacks: {ex.Message}", "red");
            return Enumerable.Empty<Stack>();
        }
    }

    public void InsertStack(Stack stack)
    {
        try
        {
            using var connection = Database.GetConnection();
            using var transaction = connection.BeginTransaction();

            connection.Execute(
                "INSERT INTO stacks (Name, CreatedDate, Duration) VALUES (@StartTime, @EndTime, @Duration)",
                new { Name = stack.Name, CreatedDate = stack.CreatedDate },
                transaction: transaction);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            Utilities.DisplayMessage($"Error inserting session: {ex.Message}", "red");
        }
    }

    public bool DeleteStack(int id)
    {
        try
        {
            using var connection = Database.GetConnection();
            using var transaction = connection.BeginTransaction();

            var recordsAffected = connection.Execute(
                "DELETE FROM stacks WHERE Id = @Id", new {Id = id}, transaction: transaction);

            if (recordsAffected > 0)
            {
                transaction.Commit();
                return true;
            }

            transaction.Rollback();
            return false;

        }
        catch (Exception ex)
        {
            Utilities.DisplayMessage($"Error deleting stack: {ex.Message}", "red");
            return false;
        }
    }
}
