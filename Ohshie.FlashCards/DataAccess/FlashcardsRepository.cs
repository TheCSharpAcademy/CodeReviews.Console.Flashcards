using System.Data.SqlClient;
using Ohshie.FlashCards.CardsManager;

namespace Ohshie.FlashCards.DataAccess;

public class FlashcardsRepository : DbBase
{
    public List<FlashCard> FetchAttachedFlashcards(int deckId)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "SELECT * FROM FlashCards " +
                                       $"WHERE DeckId = {deckId}";

            var reader = tableCommand.ExecuteReader();

            var cardsList = ReadFromDbToFlashcardList(reader);

            return cardsList;
        }
    }

    private List<FlashCard> ReadFromDbToFlashcardList(SqlDataReader reader)
    {
        List<FlashCard> cardList = new();

        while (reader.Read())
        {
            var flashCard = new FlashCard()
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Content = reader.GetString(2),
            };

            cardList.Add(flashCard);
        }

        return cardList;
    }

    public void CreateNewFlashcard(FlashCard newCard)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();
            
            tableCommand.CommandText = "INSERT INTO FlashCards (Name, Content, DeckId) " +
                                       $"VALUES ('{newCard.Name}', '{newCard.Content}', {newCard.DeckId})";

            tableCommand.ExecuteNonQuery();
        }
    }
    
    public FlashCard FetchFlashcardById(int id)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "SELECT * FROM FlashCards " +
                                       $"WHERE Id = {id}";

            var reader = tableCommand.ExecuteReader();

            var fetchedCard = ReadToCard(reader);

            return fetchedCard;
        }
    }

    private FlashCard ReadToCard(SqlDataReader reader)
    {
        var flashCard = new FlashCard();
        while (reader.Read())
        {
            flashCard.Id = reader.GetInt32(0);
            flashCard.Name = reader.GetString(1);
            flashCard.Content = reader.GetString(2);
        }

        return flashCard;
    }
    
    public void UpdateFlashcard(FlashCard flashCard, string updatedName, string updatedContent)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "UPDATE FlashCards " +
                                       $"SET Name = '{updatedName}', Content = '{updatedContent}' " +
                                       $"WHERE Id = {flashCard.Id}";
            
            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }
    
    public void DeleteFlashcard(FlashCard flashCard)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "DELETE FROM FlashCards  " +
                                       $"WHERE Id = {flashCard.Id}";
            
            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }
}