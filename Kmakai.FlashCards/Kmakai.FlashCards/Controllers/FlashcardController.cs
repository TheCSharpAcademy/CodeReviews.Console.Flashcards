using Kmakai.FlashCards.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace Kmakai.FlashCards.Controllers;

public class FlashcardController
{
    private static readonly string? ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");

    public static List<Flashcard> GetFlashcards(int stackId)
    {
        var flashcards = new List<Flashcard>();

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"
                SELECT * FROM Flashcards
                WHERE StackId = {stackId}";

            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                Flashcard flashcard = new Flashcard(reader.GetInt32(1), reader.GetString(2), reader.GetString(3))
                {
                    Id = reader.GetInt32(0)
                };
                flashcards.Add(flashcard);
            }

            connection.Close();
        }

        return flashcards;
    }

    public static void AddFlashcard(Flashcard flashcard)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$" 
                INSERT INTO Flashcards (StackId, front, back)
                VALUES ('{flashcard.StackId}', '{flashcard.Front}', '{flashcard.Back}')";

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public static void DeleteFlashcard(int id)
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$" 
                DELETE FROM Flashcards
                WHERE Id = {id}";

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
