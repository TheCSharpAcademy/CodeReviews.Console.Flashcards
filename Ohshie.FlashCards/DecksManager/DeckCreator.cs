using Ohshie.FlashCards.CardsManager;

namespace Ohshie.FlashCards.StacksManager;

public class DeckCreator
{
    private readonly DecksService _decksService = new();

    public void Create()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Creating new FlashCards Deck"));
        
        MakeNewDeck(out var newDeck);
        PopulateDeckWithCards(newDeck);
    }

    private void MakeNewDeck(out Deck creatingDeck)
    {
        creatingDeck = new()
        {
            Name = AskUser.AskTextInput(where:"Deck", what:"name"),
            Description = AskUser.AskTextInput(where:"Deck", what:"description")
        };

        _decksService.CreateNewDeck(creatingDeck);
    }

    private void PopulateDeckWithCards(Deck newDeck)
    {
        FlashcardCreator flashcardCreator = new(newDeck.Id);
        bool createCard = true;
        while (createCard)
        {
            flashcardCreator.Create();
            createCard = AnsiConsole.Confirm("Add one more card?");
        }
    }
}