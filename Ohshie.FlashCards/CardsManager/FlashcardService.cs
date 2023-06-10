namespace Ohshie.FlashCards.CardsManager;

public class FlashcardService
{
    private DbOperations _dbOperations = new();
    
    public void CreateNewFlashcard(FlashCard newCard)
    {
        if (string.IsNullOrEmpty(newCard.Name) || newCard.DeckId < 1) return;
        
        _dbOperations.CreateNewFlashcard(newCard);
    }
}