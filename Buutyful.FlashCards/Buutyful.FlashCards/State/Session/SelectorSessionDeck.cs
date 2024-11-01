using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;

namespace Buutyful.FlashCards.State.Session;

public class SelectorSessionDeck : IState
{
    private readonly StateManager _stateManager;
    private readonly DeckRepository _deckRepository = new();
    private Deck? Deck { get; set; }
    private List<Deck> Decks { get; set; } = new();
    public SelectorSessionDeck(StateManager manager)
    {
        _stateManager = manager;
        Decks = [.. _deckRepository.GetAll()];
    }
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new GameSessionState(_stateManager, Deck));
    }

    public void Render()
    {
        Console.Clear();
        Console.WriteLine("Select session deck:");
        var deckSelected = UiHelper.DisplayOptions(Decks.Select(d => d.Name));
        Deck = Decks.FirstOrDefault(d => d.Name.Equals(deckSelected))!;       
    }
}
