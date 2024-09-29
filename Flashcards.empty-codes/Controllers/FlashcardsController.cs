using Flashcards.empty_codes.Data;
using Flashcards.empty_codes.Models;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flashcards.empty_codes.Controllers;

internal class FlashcardsController
{
    private readonly Database database;

    public FlashcardsController(Database db)
    {
        database = db;
    }

    public void InsertFlashcard(FlashcardDTO card)
    {
        using var conn = new SqlConnection(database.connectionString);
        string insertQuery = "INSERT INTO Flashcards(FlashcardId, Question, Answer, StackId) VALUES (@FlashcardId, @Question, @Answer, @StackId)";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@FlashcardId", card.FlashcardId);
            cmd.Parameters.AddWithValue("@Question", card.Question);
            cmd.Parameters.AddWithValue("@Answer", card.Answer);
            cmd.Parameters.AddWithValue("@StackId", card.StackId);
            cmd.ExecuteNonQuery();
        }
        catch (SqlException e)
        {
            {
                AnsiConsole.MarkupLine($"[red]Error occurred while trying to insert your flashcard\n - Details: {e.Message}[/]");
            }
        }
    }
    public List<FlashcardDTO> ViewAllFlashcards()
    {
        var cards = new List<FlashcardDTO>();
        using var conn = new SqlConnection(database.connectionString);
        string readQuery = "SELECT * FROM Flashcards";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(readQuery, conn);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                FlashcardDTO card = new FlashcardDTO
                {
                    FlashcardId = reader.GetInt32(0),
                    Question = reader.GetString(1),
                    Answer = reader.GetString(2),
                    StackId = reader.GetInt32(3)
                };
                cards.Add(card);
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to access your cards\n - Details: {e.Message}[/]");
        }
        return cards;
    }

    public void UpdateFlashcard(FlashcardDTO card)
    {
        using var conn = new SqlConnection(database.connectionString);
        string updateQuery = "UPDATE Flashcards SET FlashcardId = @FlashcardId, Question = @Question, Answer = @Answer WHERE FlashcardId = @FlashcardId\"";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(updateQuery, conn);
            cmd.Parameters.AddWithValue("@FlashcardId", card.FlashcardId);
            cmd.Parameters.AddWithValue("@Question", card.Question);
            cmd.Parameters.AddWithValue("@Answer", card.Answer);

            int result = cmd.ExecuteNonQuery();

            if (result == 0)
            {
                AnsiConsole.MarkupLine($"[yellow]No card found with the provided Id: {card.FlashcardId}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Flashcard with Id: {card.FlashcardId} successfully updated.[/]");
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to change the card name\n - Details: {e.Message}[/]");
        }
    }

    public void DeleteFlashcard(FlashcardDTO card)
    {
        using var conn = new SqlConnection(database.connectionString);
        string deleteQuery = "DELETE FROM Flashcards WHERE FlashcardId = @FlashcardId";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(deleteQuery, conn);

            int result = cmd.ExecuteNonQuery();

            if (result == 0)
            {
                AnsiConsole.MarkupLine($"[yellow]No card found with the provided Id: {card.FlashcardId}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Flashcard with Id: {card.FlashcardId} successfully deleted.[/]");
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to delete your card\n - Details: {e.Message}[/]");
        }
    }
}
