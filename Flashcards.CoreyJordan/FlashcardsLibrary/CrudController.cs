using FlashcardsLibrary.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace FlashcardsLibrary;
public static class CrudController
{
    private static readonly string FlashCardsDB = "FlashCardsDB";

    private static string ConnectionString(string connectionString)
    {
        return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
    }

    public static List<DeckModel> GetAllDecks()
    {
        List<DeckModel> decks = new();
        using (SqlConnection connection = new(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            SqlCommand getAll = connection.CreateCommand();
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

        using (SqlConnection connection = new(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            SqlCommand getAll = connection.CreateCommand();
            getAll.CommandText = @"SELECT * 
                                FROM dbo.cards;";

            SqlDataReader reader = getAll.ExecuteReader();
            while (reader.Read())
            {
                cards.Add(new FlashCardModel()
                {
                    Front = reader.GetString(0),
                    Back = reader.GetString(1),
                    DeckId = reader.GetInt32(2)
                });
            }
        }
        return cards;
    }
    public static int InsertDeck(string newDeck)
    {
        int success;
        using (SqlConnection connection = new(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            SqlCommand insertDeck = connection.CreateCommand();
            insertDeck.CommandText = @"INSERT INTO dbo.decks (name)
                                    VALUES (@Name);";
            insertDeck.Parameters.AddWithValue("@Name", newDeck);
            success = insertDeck.ExecuteNonQuery();
        }
        return success;
    }
    public static bool DeckExists(string name)
    {
        bool deckExists = true;
        int checkQuery;

        using (SqlConnection connection = new(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            SqlCommand checkName = connection.CreateCommand();
            checkName.CommandText = @"SELECT 1 
                                    FROM dbo.decks
                                    WHERE EXISTS (
                                    SELECT 1 
                                    WHERE name = @Name
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

    public static void UpdateDeckName(string name, int id)
    {
        using (SqlConnection connection = new(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            SqlCommand updateDeck = connection.CreateCommand();
            updateDeck.CommandText = @"UPDATE dbo.decks
                                    SET name = @Name
                                    WHERE id = @Id;";
            updateDeck.Parameters.AddWithValue("@Id", id);
            updateDeck.Parameters.AddWithValue("@Name", name);
            updateDeck.ExecuteNonQuery();
        }
    }

    public static DeckModel GetDeck(string deckName)
    {
        DeckModel deck = new();

        using (SqlConnection connection = new(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            SqlCommand getDeckByName = connection.CreateCommand();
            getDeckByName.CommandText = @"SELECT * 
                                        FROM dbo.decks
                                        WHERE name = @Name;";
            getDeckByName.Parameters.AddWithValue("@Name", deckName);
            SqlDataReader reader = getDeckByName.ExecuteReader();
            while (reader.Read())
            {
                deck.Id = reader.GetInt32(0);
                deck.Name = reader.GetString(1);
                deck.Deck = GetContents(deck);
            }
        }
        return deck;
    }

    public static DeckModel GetDeck(int deckId)
    {
        DeckModel deck = new();

        using (SqlConnection connection = new(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            SqlCommand getDeckByName = connection.CreateCommand();
            getDeckByName.CommandText = @"SELECT * 
                                        FROM dbo.decks
                                        WHERE id = @ID;";
            getDeckByName.Parameters.AddWithValue("@ID", deckId);
            SqlDataReader reader = getDeckByName.ExecuteReader();
            while (reader.Read())
            {
                deck.Id = reader.GetInt32(0);
                deck.Name = reader.GetString(1);
                deck.Deck = GetContents(deck);
            }
        }
        return deck;
    }

    private static List<FlashCardModel> GetContents(DeckModel deck)
    {
        List<FlashCardModel> deckContents = new();

        using (SqlConnection connection = new(ConnectionString(FlashCardsDB)))
        {
            connection.Open();
            SqlCommand getContents = connection.CreateCommand();
            getContents.CommandText = @"SELECT *
                                    FROM flashcards
                                    WHERE deck_Id = @DeckId;";
            getContents.Parameters.AddWithValue("@DeckId", deck.Id);
            SqlDataReader reader = getContents.ExecuteReader();
            while (reader.Read())
            {
                deckContents.Add(new FlashCardModel
                {
                    Front = reader.GetString(0),
                    Back = reader.GetString(1),
                    DeckId = deck.Id,
                });
            }
        }

        return deckContents;
    }
}
