using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.CardsManager;

public class FlashcardCreator
{
    private int DeckId { get; set; }

    public FlashcardCreator(int deckId)
    {
        DeckId = deckId;
    }
    
    public void Create()
    {
        FlashCard newCard = new()
        {
            Name = GetName(),
            Content = AskUser.AskTextInput(where: "Card", what: "content of a card"),
            DeckId = DeckId
        };

        FlashcardService flashcardService = new();
        flashcardService.CreateNewFlashcard(newCard);
    }

    private string GetName()
    {
        string name;
        
        do
        {
            name = AskUser.AskTextInput(where: "Card", what: "name");
        } while (!Verify.EnteredAppropriateLength(name,100));

        return name;
    }
}