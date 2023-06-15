using System.Data;
using System.Data.SqlClient;
using Ohshie.FlashCards.CardsManager;
using Ohshie.FlashCards.DataAccess;
using Ohshie.FlashCards.StacksManager;

public class DbOperations : DbBase
{
    public void InitDb()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = SqlCommands["createStackTable"];

            tableCommand.ExecuteNonQuery();

            tableCommand.CommandText = SqlCommands["createFlashcardsTable"];
    
            tableCommand.ExecuteNonQuery();
    
            connection.Close();
        }
    }

    public List<Deck> FetchAllDecks()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = SqlCommands["fetchAllDecks"];

            var reader = tableCommand.ExecuteReader();

            var stacksList = ReadFromDbToDecksList(reader);

            return stacksList;
        }
    }
    
    private List<Deck> ReadFromDbToDecksList(SqlDataReader reader)
    {
        List<Deck> deckList = new();
        Deck deck = null;
        
        while (reader.Read())
        {
            if (!deckList.Any(d => d.Id == reader.GetInt32(0)))
            {
                deck = new Deck
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    FlashCards = new List<FlashCard>()
                };
                deckList.Add(deck);
            }

            deck.FlashCards.Add(new FlashCard
            {
                Id = reader.GetInt32(3),
                Name = reader.GetString(4),
                Content = reader.GetString(5),
                DeckId = deck.Id
            });
        }
        return deckList;
    }

    public void CreateNewDeck(Deck newDeck)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "INSERT INTO Stacks (Name, Description) " +
                                       "OUTPUT INSERTED.Id " +
                                       $"VALUES ('{newDeck.Name}', '{newDeck.Description}')";

            newDeck.Id = Convert.ToInt32(tableCommand.ExecuteScalar());
            
            connection.Close();
        }
    }

    public void CreateNewFlashcard(FlashCard newCard)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();
            
            tableCommand.CommandText = "INSERT INTO FlashCards (Name, Content, StackId) " +
                                       $"VALUES ('{newCard.Name}', '{newCard.Content}', {newCard.DeckId})";

            tableCommand.ExecuteNonQuery();
        }
    }

    public Deck FetchDeckById(int id)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = SqlCommands["fetchOneDecksById"]+id;

            var reader = tableCommand.ExecuteReader();

            var fetchedDeck = ReadToDto(reader);

            return fetchedDeck;
        }
    }

    private Deck ReadToDto(SqlDataReader reader)
    {
        Deck deck = null;

        while (reader.Read())
        {
            if (deck == null)
            {
                deck = new Deck()
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    FlashCards = new List<FlashCard>()
                };
            }
            
            deck.FlashCards.Add(new FlashCard()
            {
                Id = reader.GetInt32(3),
                Name = reader.GetString(4),
                Content = reader.GetString(5),
                DeckId = deck.Id
            });
        }
        
        return deck;
    }
}