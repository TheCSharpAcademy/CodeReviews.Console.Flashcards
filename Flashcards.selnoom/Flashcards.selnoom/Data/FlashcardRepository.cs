using Flashcards.selnoom.Models;
using Microsoft.Data.SqlClient;

namespace Flashcards.selnoom.Data;

internal class FlashcardRepository
{
    private string connectionString;

    internal FlashcardRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    internal void CreateFlashcard(int stackId, string question, string answer)
    {
        using var connection = new SqlConnection(connectionString);

        string query = @"INSERT INTO Flashcard  (StackId, Question, Answer)
                         VALUES (@stackId, @question, @answer)";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@stackId", stackId);
        command.Parameters.AddWithValue("@question", question);
        command.Parameters.AddWithValue("@answer", answer);

        connection.Open();
        command.ExecuteNonQuery();
    }

    internal List<Flashcard> GetFlashcards()
    {
        List<Flashcard> flashcards = new();
        using var connection = new SqlConnection(connectionString);
        string query = @"SELECT * FROM Flashcard";
        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            Flashcard flashcard = new Flashcard
            {
                FlashcardId = reader.GetInt32(reader.GetOrdinal("FlashcardId")),
                StackId = reader.GetInt32(reader.GetOrdinal("StackId")),
                Question = reader.GetString(reader.GetOrdinal("Question")),
                Answer = reader.GetString(reader.GetOrdinal("Answer"))
            };

            flashcards.Add(flashcard);
        }

        return flashcards;
    }

    internal List<Flashcard> GetFlashcardsByStack(int stackId)
    {
        List<Flashcard> flashcards = new();
        using var connection = new SqlConnection(connectionString);
        string query = @"SELECT * FROM Flashcard
                         WHERE StackId = @stackId";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@stackId", stackId);

        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            Flashcard flashcard = new Flashcard
            {
                FlashcardId = reader.GetInt32(reader.GetOrdinal("FlashcardId")),
                StackId = reader.GetInt32(reader.GetOrdinal("StackId")),
                Question = reader.GetString(reader.GetOrdinal("Question")),
                Answer = reader.GetString(reader.GetOrdinal("Answer"))
            };

            flashcards.Add(flashcard);
        }

        return flashcards;
    }

    internal void UpdateFlashcard(int flashcardId, string question, string answer)
    {
        using var connection = new SqlConnection(connectionString);

        string query = @"UPDATE Flashcard
                         SET Question = @question, Answer = @answer
                         WHERE FlashcardId = @flashcardId";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@flashcardId", flashcardId);
        command.Parameters.AddWithValue("@question", question);
        command.Parameters.AddWithValue("@answer", answer);

        connection.Open();
        command.ExecuteNonQuery();
    }

    internal void DeleteFlashcard(int flashcardId)
    {
        using var connection = new SqlConnection(connectionString);

        string query = @"DELETE FROM Flashcard
                         WHERE FlashcardId = @flashcardId";
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@flashcardId", flashcardId);

        connection.Open();
        command.ExecuteNonQuery();
    }
}
