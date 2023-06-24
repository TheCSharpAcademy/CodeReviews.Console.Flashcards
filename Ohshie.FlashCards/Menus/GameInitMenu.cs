using Ohshie.FlashCards.Game;
using Ohshie.FlashCards.StacksManager;

namespace Ohshie.FlashCards.Menus;

public class GameInitMenu
{
    private readonly DecksService _decksService = new();
    private List<DeckDto> _deckDtos = new();

    public void Initialize()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Rule("Lets play!"));

        if (!Verify.DeckExist("create one"))
        {
            DeckCreator creator = new();
            creator.Create();
        }
        
        AnsiConsole.Write(DecksTable());

        var userChoice = Menu();

        GameEngine gameEngine = new GameEngine(userChoice);
        
        gameEngine.Initialize();
    }

    private DeckDto? Menu()
    {
        var userChoice = Selector();

        var chosenDeck = ConvertIdAndNameToDeckDto(userChoice);
        
        return chosenDeck;
    }

    private string Selector()
    {
        return AnsiConsole.Prompt
        (
            new SelectionPrompt<string>()
                .PageSize(5)
                .Title("Select deck to solve")
                .AddChoices(CreateDecksIdAndName())
        );
    }

    private List<string> CreateDecksIdAndName()
    {
        List<string> deckIdAndNameList = new();
        
        foreach (var deck in _deckDtos)
        {
            deckIdAndNameList.Add(deck.ViewId+". "+deck.DeckName);
        }
        
        return deckIdAndNameList;
    }

    private DeckDto? ConvertIdAndNameToDeckDto(string idAndName)
    {
        string[] splittedIdAndName = idAndName.Split(".");
        return _deckDtos.FirstOrDefault(dd => dd.ViewId == Convert.ToInt32(splittedIdAndName[0]));
    }
    
    private Table DecksTable()
    {
        var table = new Table();

        table.AddColumn("Deck Id");
        table.AddColumn("Name");
        table.AddColumn("Description");
        table.AddColumn("Amount of flashcards");

        _deckDtos = _decksService.OutputDecksToDisplay();

        foreach (var deck in _deckDtos)
        {
            table.AddRow($"{deck.ViewId}",
                $"{deck.DeckName}",
                $"{deck.DeckDescription}",
                $"{deck.AmountOfFlashcards}");
        }

        return table;
    }
}