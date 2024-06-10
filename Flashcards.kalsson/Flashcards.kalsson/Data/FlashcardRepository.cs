using Dapper;
using Flashcards.kalsson.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.kalsson.Data;

public class FlashcardRepository
{
    private readonly string _connectionString;

    public FlashcardRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Flashcard> GetAllFlashcards(int stackId)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<Flashcard>("SELECT * FROM Flashcards WHERE StackId = @StackId", new { StackId = stackId });
    }

    public Flashcard GetFlashcardById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.QuerySingleOrDefault<Flashcard>("SELECT * FROM Flashcards WHERE Id = @Id", new { Id = id });
    }

    public void AddFlashcard(Flashcard flashcard)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("INSERT INTO Flashcards (StackId, Question, Answer) VALUES (@StackId, @Question, @Answer)", flashcard);
    }

    public void DeleteFlashcard(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("DELETE FROM Flashcards WHERE Id = @Id", new { Id = id });
    }
}