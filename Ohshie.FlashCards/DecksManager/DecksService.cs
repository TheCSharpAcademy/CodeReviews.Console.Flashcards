using Ohshie.FlashCards.DataAccess;

namespace Ohshie.FlashCards.StacksManager;

public class DecksService
{
    private readonly DecksRepository _decksRepository = new();

    public void CreateNewDeck(Deck newDeck)
    {
        if (string.IsNullOrEmpty(newDeck.Name)) return;
        
        _decksRepository.CreateNewDeck(newDeck);
    }

    private List<Deck> FetchAllDecksFromDb()
    {
        var decksList = _decksRepository.FetchAllDecks();
        
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
        
        _decksRepository.RenameDeck(deck, newName);
    }
    
    public void ChangeDescription(string newDescription, DeckDto deckDto)
    {
        var deck = Mapper.DeckDtoToDeckMapper(deckDto);
        
        _decksRepository.ChangeDeckDescription(deck, newDescription);
    }

    public void DeleteDeck(DeckDto deckDto)
    {
        var deck = Mapper.DeckDtoToDeckMapper(deckDto);
        
        _decksRepository.DeleteDeck(deck);
    }
}