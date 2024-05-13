using Dapper;
using FlashcardsLibrary.Models;
using Microsoft.Data.SqlClient;

namespace FlashcardsLibrary.Repositories;
public class FlashcardsRepository : IFlashCardsRepository
{
    private readonly string? connectionString;
    public FlashcardsRepository()
    {
        connectionString = AppConfig.GetFullConnectionString();
    }
    public async Task<IEnumerable<Flashcard>> GetAllAsync(Stack stack)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var getAllSql = "SELECT Id, StackId, Question, Answer FROM Flashcard WHERE StackId = @StackId";

        return await connection.QueryAsync<Flashcard>(getAllSql, new { StackId = stack.Id });
    }

    public async Task AddAsync(Flashcard flashcard)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var insertSql = "INSERT INTO Flashcard (StackId, Question, Answer) VALUES (@StackId, @Question, @Answer)";

        await connection.ExecuteAsync(insertSql, flashcard);
    }

    public async Task DeleteAsync(Flashcard flashcard)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var deleteSql = "DELETE FROM Flashcard WHERE Id = @Id";

        await connection.ExecuteAsync(deleteSql, flashcard);
    }

    public async Task UpdateAsync(Flashcard flashcard)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var updateSql = "UPDATE Flashcard SET StackId = @StackId, Question = @Question, Answer = @Answer WHERE Id = @Id";

        await connection.ExecuteAsync(updateSql, flashcard);
    }

    public async Task<bool> FlashcardExistsAsync(string question)
    {
        using var connection = new SqlConnection(connectionString);

        connection.Open();

        var checkNameSql = "SELECT TOP 1 COUNT(*) FROM Flashcard WHERE Question = @Question";

        return await connection.ExecuteScalarAsync<bool>(checkNameSql, new { Question = question });
    }
}
