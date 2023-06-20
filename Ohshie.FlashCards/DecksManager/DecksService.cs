using Ohshie.FlashCards.DataAccess;

namespace Ohshie.FlashCards.StacksManager;

public class DecksService
{
    private readonly DbOperations _dbOperations = new();
    private readonly Mapper _mapper = new();
    
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
        var deck = _mapper.DeckDtoToDeckMapper(deckDto);
        
        _dbOperations.RenameDeck(deck, newName);
    }
    
    public void ChangeDescription(string newDescription, DeckDto deckDto)
    {
        var deck = _mapper.DeckDtoToDeckMapper(deckDto);
        
        _dbOperations.ChangeDescription(deck, newDescription);
    }

    public void DeleteDeck(DeckDto deckDto)
    {
        var deck = _mapper.DeckDtoToDeckMapper(deckDto);
        
        _dbOperations.DeleteDeck(deck);
    }
}