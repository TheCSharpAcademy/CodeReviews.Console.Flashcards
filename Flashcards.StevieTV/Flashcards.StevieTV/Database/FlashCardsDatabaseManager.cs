using System.Data.SqlClient;
using Flashcards.StevieTV.Models;

namespace Flashcards.StevieTV.Database;

internal static class FlashCardsDatabaseManager
{
    private static readonly string ConnectionString = Flashcards.DatabaseManager.ConnectionString;
    private const string TableName = "Cards";

    internal static void Post(FlashCardDTO flashCard)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"INSERT INTO {TableName} (StackId, Front, Back) VALUES ({flashCard.StackId}, N'{flashCard.Front}', N'{flashCard.Back}')";
                tableCommand.ExecuteNonQuery();
            }
        }
    }
    internal static void Delete(FlashCard flashCard)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"DELETE FROM {TableName} WHERE Id = '{flashCard.Id}'";
                tableCommand.ExecuteNonQuery();
            }
        }
    }
    
    internal static void Update(FlashCard flashCard, string newFlashCardFront, string newFlashCardBack)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"UPDATE {TableName} SET Front = N'{newFlashCardFront}', BACK = N'{newFlashCardBack}' WHERE Id = '{flashCard.Id}'";
                tableCommand.ExecuteNonQuery();
            }
        }
    }

    internal static List<FlashCard> GetFlashCards(Stack stack)
    {
        List<FlashCard> flashCards = new List<FlashCard>();

        using (var connection = new SqlConnection(ConnectionString))
        {
            using (var tableCommand = connection.CreateCommand())
            {
                connection.Open();
                tableCommand.CommandText = $"SELECT * FROM {TableName} WHERE StackId = {stack.StackId}";

                using (var reader = tableCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            flashCards.Add(new FlashCard()
                            {
                                Id = reader.GetInt32(0),
                                StackId = reader.GetInt32(1),
                                Front = reader.GetString(2),
                                Back = reader.GetString(3)
                            });
                        }
                    }
                }
            }
        }

        return flashCards;
    }
}