using FlashcardsLibrary.Models;
using System.Data.SqlClient;

namespace FlashcardsLibrary.Data;
public class CardGateway : ConnManager
{
    public static void CreateCard(string cardFront, string cardBack, string packName)
    {
        throw new NotImplementedException();
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
