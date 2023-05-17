using FlashcardsLibrary.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace FlashcardsLibrary;
public static class CrudController
{
    public static List<DeckModel> GetAllDecks()
    {
        List<DeckModel> decks = new();
        using (var connection = new SqlConnection(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            var getAll = connection.CreateCommand();
            getAll.CommandText =
                "Select * FROM decks";

            SqlDataReader reader = getAll.ExecuteReader();
            while (reader.Read())
            {
                decks.Add(new DeckModel
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
        }
        return decks;
    }

    public static List<FlashCardModel> GetAllCards()
    {
        List<FlashCardModel> cards = new();
        using (var connection = new SqlConnection(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            var getAll = connection.CreateCommand();
            getAll.CommandText = "SELECT * FROM cards";

            SqlDataReader reader = getAll.ExecuteReader();
            while (reader.Read())
            {
                cards.Add(new FlashCardModel()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Front = reader.GetString(2),
                    Back = reader.GetString(3),
                    DeckId = reader.GetInt32(4)
                });
            }
        }
        return cards;
    }
    public static void InsertDeck(string newDeck)
    {
        using (var connection = new SqlConnection(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            var insertDeck = connection.CreateCommand();
            insertDeck.CommandText = $"INSERT INTO decks (name) VALUES (@Name)";
            insertDeck.Parameters.AddWithValue("@Name", newDeck);
            insertDeck.ExecuteNonQuery();
        }
    }
    public static bool DeckExists(string name)
    {
        throw new NotImplementedException();
    }

    private static readonly string FlashCardsDB = "FlashCardsDB";
    private static string ConnectionString(string connectionString)
    {
        return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
    }

}
