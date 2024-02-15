using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Spectre.Console;


namespace Buutyful.Coding_Tracker.State;

public class UpdateState(StateManager manager, Object obj) : IState
{
    private readonly StateManager _stateManager = manager;
    private readonly Object _obj = obj;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new DecksViewState(_stateManager));
    }

    public void Render()
    {
        AnsiConsole.MarkupLine("[yellow] just delete what u'd like to update and create a new one cba :)) [/]");
        
    }
}