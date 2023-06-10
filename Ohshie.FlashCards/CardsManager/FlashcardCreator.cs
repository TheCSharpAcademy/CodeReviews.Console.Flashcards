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
            Name = AskUser.AskTextInput(where: "Card", what: "name"),
            Content = AskUser.AskTextInput(where: "Card", what: "content of a card"),
            DeckId = DeckId
        };

        FlashcardService flashcardService = new();
        flashcardService.CreateNewFlashcard(newCard);
    }
}