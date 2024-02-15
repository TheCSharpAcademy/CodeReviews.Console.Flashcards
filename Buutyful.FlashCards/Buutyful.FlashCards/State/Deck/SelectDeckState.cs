using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;

namespace Buutyful.FlashCards.State;

internal class SelectDeckState : IState
{
    private readonly StateManager _stateManager;
    private readonly List<Deck> _decks;
    private readonly string _command;

    public SelectDeckState(StateManager manager, List<Deck> decks, string command)
    {
        _stateManager = manager;
        _decks = decks;
        _command = command;
    }
    public ICommand GetCommand()
    {
        var deck = SelectDeck();
        return DeckViewSelector(_command, deck);
    }

    public void Render()
    {
        Console.WriteLine("Select Deck:");
    }
    private ICommand DeckViewSelector(string cmd, Deck deck) => cmd?.ToLower() switch
    {
        "deckcards" => new SwitchStateCommand(_stateManager, new DeckCardsViewState(_stateManager, deck)),
        "updatedeck" => new SwitchStateCommand(_stateManager, new UpdateDeckState(_stateManager, deck)),
        "deletedeck" => new SwitchStateCommand(_stateManager, new DeleteDeckState(_stateManager, deck)),
        _ => new InvalidCommand(cmd, "deckviewselector")
    };
    private Deck SelectDeck()
    {
        var deckName = UiHelper.DisplayOptions(_decks.Select(d => d.Name));
        Console.WriteLine($"Selected deck: {deckName}");
        return _decks.First(d => d.Name.Equals(deckName));
    }
}
