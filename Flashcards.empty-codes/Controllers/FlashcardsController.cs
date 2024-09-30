using Flashcards.empty_codes.Data;
using Flashcards.empty_codes.Models;
using Spectre.Console;
using System.Data.SqlClient;

namespace Flashcards.empty_codes.Controllers;

internal class FlashcardsController
{
    public int CheckIfCardExists(FlashcardDTO card)
    {
        int exists = 0;
        using var conn = new SqlConnection(Database.ConnectionString);
        string checkQuery = "SELECT COUNT(*) FROM Stacks WHERE FlashcardId = @FlashcardId";
        try
        {
            conn.Open();

            using var checkCmd = new SqlCommand(checkQuery, conn);
            checkCmd.Parameters.AddWithValue("@FlashcardId", card.FlashcardId);
            exists = (int)checkCmd.ExecuteScalar();
        }
        catch (SqlException e)
        {
            {
                AnsiConsole.MarkupLine($"[red]Error occurred while trying to check if flashcard exists\n - Details: {e.Message}[/]");
            }
        }
        return exists;
    }

    public int GetFlashcardIdByQuestion(string question, int stackId)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        string query = "SELECT FlashcardId FROM Flashcards WHERE Question = @Question AND StackId = @StackId";
        int flashcardId = -1;

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Question", question);
            cmd.Parameters.AddWithValue("@StackId", stackId);

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                flashcardId = (int)result;
            }
            else
            {
                AnsiConsole.MarkupLine($"[yellow]No flashcard found with the question: '{question}' in stack ID {stackId}[/]");
                return -1;
            }
        }
        catch (SqlException e)
        {
            AnsiConsole.MarkupLine($"[red]Error occurred while trying to fetch flashcard ID by question\n - Details: {e.Message}[/]");
            return -1;
        }

        return flashcardId;
    }

    public void InsertFlashcard(FlashcardDTO card)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        string insertQuery = "INSERT INTO Flashcards(Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(insertQuery, conn);
            cmd.Parameters.AddWithValue("@Question", card.Question);
            cmd.Parameters.AddWithValue("@Answer", card.Answer);
            cmd.Parameters.AddWithValue("@StackId", card.StackId);

            int result = cmd.ExecuteNonQuery();

            if (result == 0)
            {
                AnsiConsole.MarkupLine($"[yellow]Flashcard could not be added.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Flashcard successfully added.[/]");
            }
        }
        catch (SqlException e)
        {
            {
                AnsiConsole.MarkupLine($"[red]Error occurred while trying to insert your flashcard\n - Details: {e.Message}[/]");
            }
        }
    }
    public List<FlashcardDTO> ViewAllFlashcards(StackDTO stack)
    {
        var cards = new List<FlashcardDTO>();
        using var conn = new SqlConnection(Database.ConnectionString);
        string readQuery = "SELECT * FROM Flashcards WHERE StackId = @StackId";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(readQuery, conn);
            cmd.Parameters.AddWithValue("@StackId", stack.StackId);

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
        using var conn = new SqlConnection(Database.ConnectionString);
        string updateQuery = "UPDATE Flashcards SET Question = @Question, Answer = @Answer WHERE FlashcardId = @FlashcardId";

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
            AnsiConsole.WriteLine($"Error occurred while trying to change the card name\n - Details: {e.Message}");

        }
    }

    public void DeleteFlashcard(FlashcardDTO card)
    {
        using var conn = new SqlConnection(Database.ConnectionString);
        string deleteQuery = "DELETE FROM Flashcards WHERE FlashcardId = @FlashcardId";

        try
        {
            conn.Open();
            using var cmd = new SqlCommand(deleteQuery, conn);

            cmd.Parameters.AddWithValue("@FlashcardId", card.FlashcardId);

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
