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
            Name = GetName(),
            Description = GetDescription()
        };

        _decksService.CreateNewDeck(creatingDeck);
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
    
    private string GetDescription()
    {
        string description;
        
        do
        {
            description = AskUser.AskTextInput(where:"Deck", what:"description");
        } while (!Verify.EnteredAppropriateLength(description,200));

        return description;
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