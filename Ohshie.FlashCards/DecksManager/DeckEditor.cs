namespace Ohshie.FlashCards.StacksManager;

public class DeckEditor
{
    private readonly DecksService _decksService = new();

    public void RenameDeck(DeckDto deckDto)
    {
        var newName = AskUser.AskTextInput("Deck", what: "name");

        _decksService.RenameDeck(newName, deckDto);

        deckDto.DeckName = newName;
    }
    
    public void ChangeDescription(DeckDto deckDto)
    {
        var newDescription = AskUser.AskTextInput("Deck", what: "description");
        
        _decksService.ChangeDescription(newDescription, deckDto);

        deckDto.DeckDescription = newDescription;
    }
}