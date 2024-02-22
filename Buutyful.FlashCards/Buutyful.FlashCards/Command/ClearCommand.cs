using Buutyful.Coding_Tracker.Abstraction;

namespace Buutyful.Coding_Tracker.Command;

public class ClearCommand : ICommand
{
    public void Execute()
    {
        Console.Clear();
    }
}
