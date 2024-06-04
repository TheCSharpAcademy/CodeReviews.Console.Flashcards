

using System.Data.Common;
using Dapper;
using Microsoft.Data.SqlClient;

namespace Flashcards.Models;

internal class FlashcardsDbContext
{
    internal string ConnectionString { get; init; }

    public FlashcardsDbContext(string connectionString)
    {
        ConnectionString = connectionString;
        InitDatabase();
    }

    private void InitDatabase()
    {
        CreateTables();
        //AddStack(new StackDto{ StackName = "Urdu"});
        UpdateStackById(8, new StackDto{ StackName = "Urdu12"});
    }

    private void CreateTables()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"
                IF OBJECT_ID(N'[dbo].[Stacks]', 'U') IS NULL
                CREATE TABLE Stacks(
                    Id INT NOT NULL,
                    StackName Varchar(20) NOT NULL Unique,
                    Constraint [PK_Stacks] Primary Key Clustered ([Id] Asc)
                );
                IF OBJECT_ID(N'[dbo].[Flashcards]', 'U') IS NULL
                Create Table Flashcards(
                    Id INT Not Null,
                    StackId INT NOT NULL,
                    Front VARCHAR(50) NOT NULL,
                    Back VARCHAR(50) NOT NULL,
                    CONSTRAINT [PK_FLASHCARDS] PRIMARY KEY CLUSTERED ([ID] ASC),
                    CONSTRAINT [FK1_Flashcards_Stacks] FOREIGN KEY ([StackId]) REFERENCES [dbo].[Stacks] ([Id]) ON DELETE CASCADE
                );
            ";
            connection.Execute(sql);
        }
    }

    #region Stack SQl Operations

    internal void AddStack(StackDto stack)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"INSERT INTO Stacks (StackName)
                            VALUES(@StackName);
                        ";
            int result = connection.Execute(sql, stack);
        }
    }

    internal void UpdateStackById(int id, StackDto stack)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"UPDATE Stacks
                            SET StackName = @StackName
                            WHERE Id = @Id
                        ";
            int result = connection.Execute(sql, new {Id = id, StackName = stack.StackName});
        }
    }

    internal void UpdateStackByName(StackUpdateDto stack)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"UPDATE Stacks
                            SET StackName = @StackName
                            WHERE StackName = @StackName
                        ";
            int result = connection.Execute(sql, stack);
        }
    }

    internal void DeleteStackById(int id)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"DELETE Stacks
                            WHERE Id = @Id
                        ";
            int result = connection.Execute(sql, new { Id = id });
        }
    }

    internal void DeleteStackByStackName(string stackName)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"DELETE Stacks
                            WHERE StackName = @StackName
                        ";
            int result = connection.Execute(sql, new { StackName = stackName });
        }
    }

    internal Stack? GetStackById(int id)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"SELECT * FROM Stacks
                            WHERE Id = @Id
                        ";
            Stack? stack = connection.QueryFirstOrDefault<Stack>(sql, new { Id = id });
            return stack;
        }
    }

    internal IEnumerable<Stack>? GetAllStacks()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"SELECT * FROM Stacks ORDER BY StackName";
            IEnumerable<Stack>? stacks = connection.Query<Stack>(sql);
            return stacks;
        }
    }

    #endregion // End of Stack SQL Operations

    #region FlashCards SQL Operations

    internal void AddFlashcard(int stackId, string front, string back)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"INSERT INTO Flashcards (StackId, Front, Back)
                            VALUES(@StackId, @Front, @Back);
                        ";
            int result = connection.Execute(sql, new { StackId = stackId, Front = front, Back = back });
        }
    }

    internal void UpdateFlashcard(int id, int stackId, string front, string back)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"UPDATE Flashcards 
                        SET
                            StackId = @StackId, 
                            Front = @Front , 
                            Back = @Back
                        WHERE Id = @Id
                        ;";
            int result = connection.Execute(sql, new { Id = id, StackId = stackId, Front = front, Back = back });
        }
    }

    internal void DeleteFlashcard(int id)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"DELETE FROM Flashcards WHERE Id = @Id;";
            int result = connection.Execute(sql, new { Id = id});
            Console.WriteLine($"Result: {result}");
        }
    }

    internal Flashcard? GetFlashcardById(int id)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"SELECT * FROM Flashcards
                            WHERE Id = @Id
                        ";
            Flashcard? flashcard = connection.QueryFirstOrDefault<Flashcard>(sql, new { Id = id });
            return flashcard;
        }
    }

    internal IEnumerable<Flashcard>? GetAllFlashcards()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"SELECT * FROM Flashcards ORDER BY StackId";
            IEnumerable<Flashcard>? flashcards = connection.Query<Flashcard>(sql);
            return flashcards;
        }
    }

    #endregion


}