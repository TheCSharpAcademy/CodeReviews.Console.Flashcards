using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Spectre.Console;

namespace Buutyful.Coding_Tracker.State;

public class ViewState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new MainMenuState(_stateManager));
    }

    public void Render()
    {
        AnsiConsole.MarkupLine("[gray]=====LOADING HABITS=====[/]");

        var table = new Table();
        // Add some columns
        table.AddColumn("Id");
        table.AddColumn("Start");
        table.AddColumn("End");
        table.AddColumn("Duration");

        //foreach (var session in _stateManager.DbContext.Get())
        //{
        //    // Add some rows
        //    table.AddRow($"{session.Id}", $"{session.StartAt}",
        //        $"{session.EndAt}", $"{session.Duration}");
        //}
        AnsiConsole.Write(table);
        AnsiConsole.MarkupLine("[gray]========================[/]");
    }
}
