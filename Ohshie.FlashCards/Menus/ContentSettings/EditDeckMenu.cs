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

        string userChoice = MenuBuilder(5);
        
        switch (userChoice)
        {
            case "Rename Deck":
            {
                _deckEditor.RenameDeck(DeckDto);
                break;
            }
            case "Change Description":
            {
                _deckEditor.ChangeDescription(DeckDto);
                break;
            }
            case "Edit attached FlashCards":
            {
                ChooseFlashCardToEditMenu chooseFlashCardToEditMenu = new(DeckDto);
                chooseFlashCardToEditMenu.Initialize();
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
}