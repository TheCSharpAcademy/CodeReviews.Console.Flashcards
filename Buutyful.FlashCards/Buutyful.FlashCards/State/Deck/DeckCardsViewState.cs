using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;
using Spectre.Console;

namespace Buutyful.FlashCards.State;

public class DeckCardsViewState : IState
{
    private readonly Deck _deck;
    private readonly StateManager _manager;
    private readonly CardRepository _cardRepository = new();
    private readonly List<Commands> commands = new()
    {
        Commands.CreateCard,
        Commands.UpdateCard,
        Commands.DeleteCard,
        Commands.Menu,
        Commands.Back,
        Commands.Clear,
        Commands.Quit
    };
    private List<FlashCard> Cards { get; set; } = new();
    public DeckCardsViewState(StateManager manager, Deck deck)
    {
        _manager = manager;
        _deck = deck;
        Cards = [.. _cardRepository.GetByDeckId(deck.Id)];
    }
    public ICommand GetCommand()
    {
        var command = UiHelper.DisplayOptions(commands.CommandsToStrings());
        return UiHelper.MenuSelector(command, _manager);
    }

    public void Render()
    {
        Console.Clear();
        AnsiConsole.MarkupLine("[gray]=====Cards=====[/]");

        var table = new Table();
        // Add some columns       
        table.AddColumn("Name");
        table.AddColumn("Category");
        table.AddColumn("FrontQuestion");
        table.AddColumn("BackAnswer");
        table.AddColumn("CardId");

        for (int i = 0; i < Cards.Count; i++)
        {
            // Add some rows
            table.AddRow($"{_deck.Name}", $"{_deck.Category}", $"{Cards[i].FrontQuestion}", $"{Cards[i].BackAnswer}", $"{i + 1}");
        }
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[gray]========================[/]");
    }
}
