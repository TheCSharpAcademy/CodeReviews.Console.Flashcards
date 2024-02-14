using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Spectre.Console;


namespace Buutyful.Coding_Tracker.State;

public class UpdateState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new ViewState(_stateManager));
    }

    public void Render()
    {
        AnsiConsole.MarkupLine("[yellow] just delete what u'd like to update and create a new one cba :)) [/]");
    }
}