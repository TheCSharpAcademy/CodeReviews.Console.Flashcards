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
                "Select * FROM dbo.decks";

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
            getAll.CommandText = "SELECT * FROM dbo.cards";

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
            insertDeck.CommandText = $"INSERT INTO dbo.decks (name) VALUES (@Name)";
            insertDeck.Parameters.AddWithValue("@Name", newDeck);
            insertDeck.ExecuteNonQuery();
        }
    }
    public static bool DeckExists(string name)
    {
        bool deckExists = true;
        int checkQuery;

        using (var connection = new SqlConnection(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            var checkName = connection.CreateCommand();
            checkName.CommandText = @"SELECT 1 FROM dbo.decks
                                    WHERE EXISTS (
                                    SELECT 1 WHERE name = @Name
                                    );";
            checkName.Parameters.AddWithValue("@Name", name);
            checkQuery = Convert.ToInt32(checkName.ExecuteScalar());
        }

        if (checkQuery == 0)
        {
            deckExists = false;
        }

        return deckExists;
    }

    private static readonly string FlashCardsDB = "FlashCardsDB";
    private static string ConnectionString(string connectionString)
    {
        return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
    }

}
