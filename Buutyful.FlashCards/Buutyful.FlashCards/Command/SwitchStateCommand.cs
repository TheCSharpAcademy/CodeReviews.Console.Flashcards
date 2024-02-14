using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.State;

namespace Buutyful.Coding_Tracker.Command;

public class SwitchStateCommand(StateManager manager, IState newState) : ICommand
{
    private readonly StateManager _manager = manager;
    private readonly IState _newState = newState;

    public void Execute()
    {
        _manager.SwitchState(_newState);
    }
}