namespace Ohshie.FlashCards.StacksManager;

public class DecksService
{
    private DbOperations _dbOperations = new();
    
    public void CreateNewDeck(Deck newDeck)
    {
        if (string.IsNullOrEmpty(newDeck.Name)) return;
        
        _dbOperations.CreateNewDeck(newDeck);
    }
}