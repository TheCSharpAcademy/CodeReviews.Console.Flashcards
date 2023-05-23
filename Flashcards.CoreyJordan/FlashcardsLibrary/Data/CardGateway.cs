using FlashcardsLibrary.Models;
using System.Data.SqlClient;

namespace FlashcardsLibrary.Data;
public class CardGateway : ConnManager
{
    public static void CreateCard(string cardFront, string cardBack, string packName)
    {
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand createCard = connection.CreateCommand();
            createCard.CommandText = @"INSERT INTO flashcards
                                    (front, back, deck_id)
                                    VALUES (@Front, @Back, (SELECT id
                                    FROM dbo.decks
                                    WHERE name = @Name));";
            createCard.Parameters.AddWithValue("@Front", cardFront);
            createCard.Parameters.AddWithValue("@Back", cardBack);
            createCard.Parameters.AddWithValue("@Name", packName);
            createCard.ExecuteNonQuery();
        }
    }

    public static int DeleteCard(string cardFace)
    {
        int passFail;
        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand deleteCard = connection.CreateCommand();
            deleteCard.CommandText = @"DELETE FROM dbo.flashcards
                                    WHERE front = @Front;";
            deleteCard.Parameters.AddWithValue("@Front", cardFace);
            passFail = deleteCard.ExecuteNonQuery();
        }
        return passFail;
    }

    public static List<CardModel> GetPackContents(string packChoice)
    {
        List<CardModel> cards = new();

        using (SqlConnection connection = new(FlashCardDb))
        {
            connection.Open();
            SqlCommand getContents = connection.CreateCommand();
            getContents.CommandText = @"SELECT * FROM dbo.flashcards
                                        WHERE deck_id IN (
                                        SELECT id FROM dbo.decks
                                        WHERE name = @Name);";
            getContents.Parameters.AddWithValue("@Name", packChoice);
            SqlDataReader reader = getContents.ExecuteReader();
            while (reader.Read())
            {
                cards.Add(new CardModel()
                {
                    Id = reader.GetInt32(0),
                    Question = reader.GetString(1),
                    Answer = reader.GetString(2),
                    DeckId = reader.GetInt32(3),
                    DeckName = packChoice
                });
            }
        }
        return cards;
    }
}
