using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.Coding_Tracker.State;

public class DeckViewState : IState
{
    private readonly StateManager _stateManager;
    private readonly DeckRepository _deckRepository = new();
    private readonly List<Commands> commands = new()
    {
        Commands.DeckCards,        
        Commands.UpdateDeck,
        Commands.DeleteDeck,
        Commands.Menu,
        Commands.Back,
        Commands.Clear,
        Commands.Quit
    };
    private List<Deck> Decks { get; set; } = new();

    public DeckViewState(StateManager manager)
    {
        _stateManager = manager;
        Decks = _deckRepository.GetDecksOnly();
    }
    public ICommand GetCommand()
    {
        var deckName = UiHelper.DisplayOptions(Decks.Select( d => d.Name)).ToLower();
        Console.WriteLine($"Selected deck: {deckName}");
        var command = UiHelper.DisplayOptions(commands.CommandsToStrings());
        return UiHelper.MenuSelector(command, _stateManager);
    }

    public void Render()
    {        
        AnsiConsole.MarkupLine("[gray]=====Decks=====[/]");

        var table = new Table();
        // Add some columns       
        table.AddColumn("Name");
        table.AddColumn("Category");

        foreach (var deck in Decks)
        {
            // Add some rows
            table.AddRow($"{deck.Name}", $"{deck.Category}");                
        }
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[gray]========================[/]");
    }
}
