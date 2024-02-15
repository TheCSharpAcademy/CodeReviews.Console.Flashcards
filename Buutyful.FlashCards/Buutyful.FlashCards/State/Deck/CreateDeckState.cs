using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Repository;

namespace Buutyful.FlashCards.State;

public class CreateDeckState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    private readonly DeckRepository _deckRepository = new();

    public ICommand GetCommand()
    {
        throw new NotImplementedException();
    }

    public void Render()
    {
        Console.WriteLine("Create Deck State");
    }
}
