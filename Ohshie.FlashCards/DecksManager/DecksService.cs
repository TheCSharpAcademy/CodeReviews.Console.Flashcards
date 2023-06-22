using Ohshie.FlashCards.DataAccess;

namespace Ohshie.FlashCards.StacksManager;

public class DecksService
{
    private readonly DbOperations _dbOperations = new();

    public void CreateNewDeck(Deck newDeck)
    {
        if (string.IsNullOrEmpty(newDeck.Name)) return;
        
        _dbOperations.CreateNewDeck(newDeck);
    }

    private List<Deck> FetchAllDecksFromDb()
    {
        var decksList = _dbOperations.FetchAllDecks();
        
        return decksList;
    }

    public List<DeckDto> OutputDecksToDisplay()
    {
        var deckList = FetchAllDecksFromDb();
        
        int counter = 0;
        List<DeckDto> deckDtos = new();
        
        foreach (var deck in deckList)
        {
            deckDtos.Add(Mapper.DeckToDtoMapper(deck,++counter));
        }

        return deckDtos;
    }

    public void RenameDeck(string newName, DeckDto deckDto)
    {
        var deck = Mapper.DeckDtoToDeckMapper(deckDto);
        
        _dbOperations.RenameDeck(deck, newName);
    }
    
    public void ChangeDescription(string newDescription, DeckDto deckDto)
    {
        var deck = Mapper.DeckDtoToDeckMapper(deckDto);
        
        _dbOperations.ChangeDeckDescription(deck, newDescription);
    }

    public void DeleteDeck(DeckDto deckDto)
    {
        var deck = Mapper.DeckDtoToDeckMapper(deckDto);
        
        _dbOperations.DeleteDeck(deck);
    }
}