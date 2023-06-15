namespace Ohshie.FlashCards.StacksManager;

public class DecksService
{
    private DbOperations _dbOperations = new();
    
    public void CreateNewDeck(Deck newDeck)
    {
        if (string.IsNullOrEmpty(newDeck.Name)) return;
        
        _dbOperations.CreateNewDeck(newDeck);
    }

    public List<Deck> FetchAllDecksFromDb()
    {
        var decksList = _dbOperations.FetchAllDecks();
        
        return decksList;
    }

    public bool DeckExist(int id)
    {
        var deck = _dbOperations.FetchDeckById(id);
        if (deck == null || deck.Id < 1) return false;

        return true;
    }
}