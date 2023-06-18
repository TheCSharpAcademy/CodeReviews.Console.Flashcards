using Ohshie.FlashCards.DataAccess;

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

    public List<DeckDto> OutputDecksToDisplay()
    {
        var deckList = FetchAllDecksFromDb();
        
        Mapper mapper = new();
        int counter = 0;
        List<DeckDto> deckDtos = new();
        
        foreach (var deck in deckList)
        {
            deckDtos.Add(mapper.DeckToDtoMapper(deck,++counter));
        }

        return deckDtos;
    }

    public void RenameDeck(string newName, DeckDto deckDto)
    {
        Mapper mapper = new();
        
        var deck = mapper.DeckDtoToDeckMapper(deckDto);
        
        _dbOperations.RenameDeck(deck, newName);
    }
    
    public void ChangeDescription(string newDescription, DeckDto deckDto)
    {
        Mapper mapper = new();
        
        var deck = mapper.DeckDtoToDeckMapper(deckDto);
        
        _dbOperations.ChangeDescription(deck, newDescription);
    }
}