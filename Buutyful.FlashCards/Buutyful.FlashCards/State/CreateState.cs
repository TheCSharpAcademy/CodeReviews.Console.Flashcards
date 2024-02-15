using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.Command;
using Spectre.Console;

namespace Buutyful.Coding_Tracker.State;

public class CreateState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;
    public ICommand GetCommand()
    {
        return new SwitchStateCommand(_stateManager, new DecksViewState(_stateManager));
    }

    public void Render()
    {
        bool toBeSelected = true;
        int count = 0;
        while (toBeSelected)
        {
            if (count > 5)
            {
                AnsiConsole.MarkupLine("[yellow]to many attempts[/]");
                return;
            }
            AnsiConsole.MarkupLine($".");

            var startTime = Console.ReadLine();
            if (startTime?.ToLower() == "break") return;
            if (!DateTime.TryParse(startTime, out var start))
            {
                AnsiConsole.MarkupLine("[yellow]Wrong date time format[/]");
                continue;
            }

            AnsiConsole.MarkupLine($"");

            var endTime = Console.ReadLine();
            if (endTime?.ToLower() == "break") return;
            if (!DateTime.TryParse(endTime, out var end))
            {
                AnsiConsole.MarkupLine("[yellow]Wrong date time format[/]");
                continue;
            }
            if (start > end)
            {
                AnsiConsole.MarkupLine("[yellow]Start was greater than End[/]");
                continue;
            }
            toBeSelected = false;
            //_stateManager.DbContext.Create(CodingSession.Create(start, end));
            count++;
        }
    }
}

