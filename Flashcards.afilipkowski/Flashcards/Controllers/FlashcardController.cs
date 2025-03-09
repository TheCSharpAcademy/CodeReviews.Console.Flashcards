using System.Configuration;
using Dapper;
using Flashcards.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Flashcards.Controllers;

internal class FlashcardController
{
    private string connectionString;

    internal FlashcardController()
    {
        connectionString = ConfigurationManager.ConnectionStrings["dbString2"].ConnectionString;
    }

    internal List<Flashcard> GetFlashcards(int stackId)
    {
        List<Flashcard> flashcards = new();
        var sql = "SELECT * FROM Cards WHERE StackId = @StackId";
        using (var connection = new SqlConnection(connectionString))
        {
            flashcards = connection.Query<Flashcard>(sql, new { StackId = stackId }).ToList();
        }
        return flashcards;
    }

    internal void AddFlashcard(string term, string definition, int stackId)
    {
        var sql = "INSERT INTO Cards (Id, StackId, Term, Definition) VALUES (@Id, @StackId, @Term, @Definition)";
        int id = GetFlashcardCount(stackId) + 1;
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute(sql, new { Id = id, StackId = stackId, Term = term, Definition = definition });
        }
    }

    internal void EditFlashcard(string term, string definition, int stackId, int Id)
    {
        var sql = "UPDATE Cards SET Term = @Term, Definition = @Definition WHERE Id = @Id AND StackId = @StackId";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute(sql, new { Term = term, Definition = definition, Id = Id, StackId = stackId });
        }
    }

    internal void DeleteFlashcard(int stackId, int id)
    {
        var sql = "DELETE FROM Cards WHERE Id = @Id AND StackId = @StackId";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute(sql, new { Id = id, StackId = stackId });
        }
    }

    internal void SetCorrectId(int stackId, int id)
    {
        var sql = "UPDATE Cards SET Id = @NewId WHERE Id = @Id AND StackId = @StackId";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Execute(sql, new { NewId = id - 1, Id = id, StackId = stackId });
        }
    }

    internal int GetFlashcardCount(int stackId)
    {
        int id;
        var sql = $@"SELECT COUNT(*) FROM Cards
                     WHERE StackId = @stackId";
        using (var connection = new SqlConnection(connectionString))
        {
            id = connection.ExecuteScalar<int>(sql, new { stackId = stackId });
        }
        return id;
    }
}