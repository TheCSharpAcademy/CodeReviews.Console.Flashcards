using Ohshie.FlashCards.DataAccess;

namespace Ohshie.FlashCards.StacksManager;

public class DeckEditor
{
    private DecksService _decksService = new();

    public string RenameDeck(DeckDto deckDto)
    {
        var newName = AskUser.AskTextInput("Deck", what: "name");

        _decksService.RenameDeck(newName, deckDto);
        return newName;
    }
    
    public string ChangeDescription(DeckDto deckDto)
    {
        var newDescription = AskUser.AskTextInput("Deck", what: "description");

        _decksService.ChangeDescription(newDescription, deckDto);
        return newDescription;
    }
}