using Flashcards.Models;
using Microsoft.Data.SqlClient;

namespace Flashcard_Application.DataServices;

public class FlashcardDatabaseServices
{
    public static void InsertFlashcard(Flashcard flashcard) //THIS IS WORKING FINE!
    {
        using (var connection = new SqlConnection(DatabaseConfig.dbFilePath))
        {
            connection.Open();

            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = "INSERT INTO Flashcards (Question, Answer, StackId) VALUES (@Question, @Answer, @StackId)";
            sqlCommand.Parameters.AddWithValue("@Question", flashcard.Question);
            sqlCommand.Parameters.AddWithValue("@Answer", flashcard.Answer);
            sqlCommand.Parameters.AddWithValue("@StackId", flashcard.StackId);

            sqlCommand.ExecuteNonQuery();
        }
    }

    public static List<Flashcard> GetFlashCardsInStack(string StackName)
    {
        int stackId = StackDatabaseServices.GetStackID(StackName);
        List<Flashcard> listCards = new List<Flashcard>();

        using (var connection = new SqlConnection(DatabaseConfig.dbFilePath))
        {
            connection.Open();
            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = "SELECT * FROM Flashcards WHERE StackId = @stackId";
            sqlCommand.Parameters.AddWithValue("@stackId", stackId);
            sqlCommand.ExecuteNonQuery();

            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                listCards.Add(new Flashcard
                {
                    CardId = reader.GetInt32(0),
                    StackId = reader.GetInt32(1),
                    Question = reader.GetString(2),
                    Answer = reader.GetString(3),
                });
            }
        }
        return listCards;
    }

    public static void DeleteCard(string question)
    {
        using (var connection = new SqlConnection(DatabaseConfig.dbFilePath))
        {
            connection.Open();
            using var sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = "DELETE FROM Flashcards WHERE Question = @Question";
            sqlCommand.Parameters.AddWithValue("@Question", question);
            sqlCommand.ExecuteNonQuery();
        }
    }
}
