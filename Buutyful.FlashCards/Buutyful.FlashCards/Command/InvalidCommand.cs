using Buutyful.Coding_Tracker.Abstraction;

namespace Buutyful.Coding_Tracker.Command;

public class InvalidCommand(string? command, string error) : ICommand
{    
    public void Execute()
    {
        Console.WriteLine($"Invalid Command : [{command}], {error}");            
    }
}