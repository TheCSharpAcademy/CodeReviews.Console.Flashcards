using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Models;

namespace Buutyful.FlashCards.State;

public class DeleteDeckState(StateManager manager, Deck deck) : IState
{
    public ICommand GetCommand()
    {
        throw new NotImplementedException();
    }

    public void Render()
    {
        throw new NotImplementedException();
    }
}
