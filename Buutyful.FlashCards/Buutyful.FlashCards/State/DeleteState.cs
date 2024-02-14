using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Spectre.Console;

namespace Buutyful.Coding_Tracker.State;

public class DeleteState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new ViewState(_stateManager));
    }

    public void Render()
    {
    //    int count = 0;
    //    while (true)
    //    {
    //        if (count > 5)
    //        {
    //            AnsiConsole.MarkupLine("[yellow]to many attempts[/]");
    //            return;
    //        }
    //        AnsiConsole.MarkupLine("[red]Delete[/] session," +
    //            " instert the session [red][[guid]][/]:");
    //        if (Guid.TryParse(Console.ReadLine(), out var i))
    //        {
    //            //var session = _stateManager.DbContext.GetById(i);
    //            if (session != null)
    //            {
    //                Console.WriteLine($"Do you want to delete" +
    //                    $" [{_stateManager.DbContext.GetById(i)}]? [y] / [n]");
    //                var answer = Console.ReadLine();
    //                if (answer?.ToLower() == "break") return;
    //                if (answer?.ToLower() == "y")
    //                {
    //                    //_stateManager.DbContext.Delete(i);
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                AnsiConsole.MarkupLine("[yellow]session not found[/]");
    //                Console.WriteLine("Continue? [y] / [n]");
    //                var answer = Console.ReadLine();
    //                if (answer?.ToLower() != "y") return;
    //            }
    //        }
    //        count++;
    //    }
    }
}
