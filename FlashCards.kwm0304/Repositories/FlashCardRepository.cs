using FlashCards.kwm0304.Config;
using FlashCards.kwm0304.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace FlashCards.kwm0304.Repositories;

public class FlashCardRepository
{
    private readonly string _connString;
    public FlashCardRepository()
    {
        _connString = AppConfiguration.GetConnectionString("DefaultConnection"); ;
    }

    public async Task<List<FlashCard>> GetAllFlashcardsAsync(int stackId)
    {
        List<FlashCard> flashcards = [];
        var sql = "SELECT * FROM Flashcards WHERE StackId = @stackId";
        using var connection = new SqlConnection(_connString);
        await connection.OpenAsync();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@stackId", stackId);
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var flashcard = new FlashCard(
                reader["Question"].ToString() ?? throw new InvalidOperationException("Question cannot be null"),
                reader["Answer"].ToString() ?? throw new InvalidOperationException("Answer cannot be null"),
                Convert.ToInt32(reader["StackId"])
            )
            {
                FlashCardId = Convert.ToInt32(reader["FlashCardId"])
            };
            flashcards.Add(flashcard);
        }
        return flashcards;
    }

    public async Task CreateFlashcard(FlashCard flashcard)
    {
        var sql = "INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";
        using var connection = new SqlConnection(_connString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Question", flashcard.Question);
        command.Parameters.AddWithValue("@Answer", flashcard.Answer);
        command.Parameters.AddWithValue("@StackId", flashcard.StackId);
        try
        {
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
            AnsiConsole.WriteLine("Flashcard created successfully");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }
    public async Task DeleteFlashcardAsync(int id)
    {
        var sql = "DELETE FROM Flashcards WHERE FlashCardId = @Id";
        using var connection = new SqlConnection(_connString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        try
        {
            await connection.OpenAsync();
            command.ExecuteNonQuery();
            AnsiConsole.WriteLine("Flashcard successfully deleted");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }

    public async Task EditFlashardAsync(FlashCard flashcard)
    {
        var sql = "UPDATE Flashcards SET Question = @Question, Answer = @Answer WHERE FlashCardId = @Id";
        using var connection = new SqlConnection(_connString);
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Question", flashcard.Question);
        command.Parameters.AddWithValue("@Answer", flashcard.Answer);
        command.Parameters.AddWithValue("@Id", flashcard.FlashCardId);
        try
        {
            await connection.OpenAsync();
            command.ExecuteNonQuery();
            AnsiConsole.WriteLine("Flashcard updated successfully");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
        }
    }
}