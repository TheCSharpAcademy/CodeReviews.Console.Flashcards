using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.FlashCards.Helper;
using Spectre.Console;

namespace Buutyful.Coding_Tracker.Command;

public class InfoCommand : ICommand
{
    public void Execute()
    {
        Console.WriteLine("============================");
        AnsiConsole.MarkupLine("[yellow]Commands[/]:");        
        foreach (var info in UiHelper.MapCommands)
        {
            AnsiConsole.MarkupLineInterpolated($"[yellow][[{info.Key}]][/]: {info.Value}");
        }       
        Console.WriteLine("============================");       
    }
}
public enum Commands
{
    Info,
    Menu,
    View,
    Create,
    Update,
    Delete,
    Back,
    Forward,
    Clear,
    Break,
    Quit
}