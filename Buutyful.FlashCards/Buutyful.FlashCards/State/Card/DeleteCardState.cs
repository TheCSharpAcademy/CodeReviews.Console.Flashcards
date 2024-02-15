using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;
using Buutyful.FlashCards.Models;
using Buutyful.FlashCards.Repository;

namespace Buutyful.FlashCards.State;

public class DeleteCardState : IState
{
    private readonly FlashCard _card;
    private readonly StateManager _manager;
    private readonly CardRepository _cardRepository = new();
  
    public DeleteCardState(StateManager manager, FlashCard card)
    {
        _manager = manager;
        _card = card;
    }
    public ICommand GetCommand()
    {
        throw new NotImplementedException();
    }

    public void Render()
    {
        throw new NotImplementedException();
    }
}