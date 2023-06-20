using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.Menus;

public class EditDeckMenu : MenuBase
{
    private DeckDto DeckDto { get; set; }
    private readonly DeckEditor _deckEditor = new();
    
    public EditDeckMenu(DeckDto deckDto)
    {
        DeckDto = deckDto;
    }
    
    protected override string[] MenuItems { get; } =
    {
        "Rename Deck",
        "Change Description",
        "Edit attached FlashCards",
        "Delete Deck",
        "Go back"
    };

    protected override bool Menu()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Settings"));
        AnsiConsole.Write(DeckDisplay());

        string userChoice = MenuBuilder(3);
        
        switch (userChoice)
        {
            case "Rename Deck":
            {
                RenameDeck();
                break;
            }
            case "Change Description":
            {
                ChangeDescription();
                break;
            }
            case "Edit attached FlashCards":
            {
                break;
            }
            case "Delete Deck":
            {
                DecksService decksService = new();
                decksService.DeleteDeck(DeckDto);
                return true;
            }
            case "Go back":
            {
                return true;
            }
        }

        return false;
    }

    private Table DeckDisplay()
    {
        var table = new Table();
        
        table.AddColumn("Name");
        table.AddColumn("Description");
        table.AddColumn("Amount of flashcards");
        
        table.AddRow($"{DeckDto.DeckName}",
            $"{DeckDto.DeckDescription}",
            $"{DeckDto.AmountOfFlashcards}");

        return table;
    }

    private void RenameDeck()
    {
        DeckDto.DeckName = _deckEditor.RenameDeck(DeckDto);
    }

    private void ChangeDescription()
    {
        DeckDto.DeckDescription = _deckEditor.ChangeDescription(DeckDto);
    }
}