using Dapper;
using Flashcards.Dejmenek.DataAccess.Interfaces;
using Flashcards.Dejmenek.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Flashcards.Dejmenek.DataAccess.Repositories;

public class StacksRepository : IStacksRepository
{
    private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["LocalDbConnection"].ConnectionString;
    public void AddStack(string name)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"INSERT INTO Stacks (Name) VALUES
                               (@Name)";

            connection.Execute(sql, new
            {
                Name = name
            });
        }
    }

    public void DeleteFlashcardFromStack(int flashcardId, int stackId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"DELETE FROM Flashcards
                               WHERE Id = @Id AND StackId = @StackId";

            connection.Execute(sql, new
            {
                Id = flashcardId,
                StackId = stackId
            });
        }
    }

    public IEnumerable<Flashcard> GetFlashcardsByStackId(int stackId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"SELECT * FROM Flashcards WHERE StackId = @StackId";

            return connection.Query<Flashcard>(sql, new
            {
                StackId = stackId
            });
        }
    }

    public void UpdateFlashcardInStack(int flashcardId, int stackId, string front, string back)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"UPDATE Flashcards
                               SET Front = @Front, Back = @Back
                               WHERE Id = @Id AND StackId = @StackId";

            connection.Execute(sql, new
            {
                Front = front,
                Back = back,
                Id = flashcardId,
                StackId = stackId
            });
        }
    }

    public void DeleteStack(int id)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"DELETE FROM Stacks
                               WHERE Id = @Id";

            connection.Execute(sql, new
            {
                Id = id
            });
        }
    }

    public IEnumerable<Stack> GetAllStacks()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"SELECT * FROM Stacks";

            return connection.Query<Stack>(sql);
        }
    }

    public Stack GetStack(string name)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"SELECT * FROM Stacks WHERE Name = @Name";

            return connection.QuerySingle<Stack>(sql, new
            {
                Name = name
            });
        }
    }

    public bool StackExistsWithName(string name)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"IF EXISTS (
                                  SELECT 1 FROM Stacks WHERE Name = @Name
                               )
                               BEGIN
                                  SELECT 1;
                               END
                               ELSE
                               BEGIN
                                  SELECT 0;
                               END;";

            return connection.QuerySingle<bool>(sql, new
            {
                Name = name
            });
        }
    }

    public bool StackExistsWithId(int stackId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"IF EXISTS (
                                  SELECT 1 FROM Stacks WHERE Id = @Id
                               )
                               BEGIN
                                  SELECT 1;
                               END
                               ELSE
                               BEGIN
                                  SELECT 0;
                               END;";

            return connection.QuerySingle<bool>(sql, new
            {
                Id = stackId
            });
        }
    }

    public bool HasStack()
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"IF EXISTS (
                                  SELECT 1 FROM Stacks
                               )
                               BEGIN
                                  SELECT 1;
                               END
                               ELSE
                               BEGIN
                                  SELECT 0;
                               END;";

            return connection.QuerySingle<bool>(sql);
        }
    }

    public bool HasStackAnyFlashcards(int stackId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"IF EXISTS (
                                  SELECT 1 FROM Stacks s JOIN Flashcards f ON s.Id = f.StackId WHERE s.Id = @Id
                               )
                               BEGIN
                                  SELECT 1;
                               END
                               ELSE
                               BEGIN
                                  SELECT 0;
                               END;";

            return connection.QuerySingle<bool>(sql, new
            {
                Id = stackId
            });
        }
    }

    public int GetFlashcardsCountInStack(int stackId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            string sql = @"SELECT COUNT(Id) FROM Flashcards WHERE StackId = @StackId";

            return connection.QuerySingle<int>(sql, new
            {
                StackId = stackId
            });
        }
    }
}
