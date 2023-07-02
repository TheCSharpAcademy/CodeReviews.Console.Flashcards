namespace Ohshie.FlashCards.StacksManager;

public class DeckEditor
{
    private readonly DecksService _decksService = new();

    public void RenameDeck(DeckDto deckDto)
    {
        var newName = GetName();

        _decksService.RenameDeck(newName, deckDto);

        deckDto.DeckName = newName;
    }
    
    private string GetName()
    {
        string name;
        
        do
        {
            name = AskUser.AskTextInput(where:"Deck", what:"name");
        } while (!Verify.EnteredAppropriateLength(name,50));

        return name;
    }
    
    public void ChangeDescription(DeckDto deckDto)
    {
        var newDescription = GetDescription();
        
        _decksService.ChangeDescription(newDescription, deckDto);

        deckDto.DeckDescription = newDescription;
    }
    
    private string GetDescription()
    {
        string description;
        
        do
        {
            description = AskUser.AskTextInput(where:"Deck", what:"description");
        } while (!Verify.EnteredAppropriateLength(description,200));

        return description;
    }
}