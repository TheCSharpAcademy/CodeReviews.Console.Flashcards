using Buutyful.Coding_Tracker.Abstraction;

namespace Buutyful.Coding_Tracker.Command;

internal class QuitCommand : ICommand
{
    public void Execute()
    {
        Environment.Exit(0);
    }
}
