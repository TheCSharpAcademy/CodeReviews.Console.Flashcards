using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Game;
using Buutyful.FlashCards.Models;

namespace Buutyful.FlashCards.State.Session;

public class GameSessionState(StateManager manger, Deck deck) : IState
{
    private readonly StateManager _manger = manger;
    private readonly Deck _deck = deck;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_manger, new ViewSessionsState(_manger));
    }

    public void Render()
    {
        Console.Clear();
        var game = new GameManager(_deck);
        game.Run();
    }
}
