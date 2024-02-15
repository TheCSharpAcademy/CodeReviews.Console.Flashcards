using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.FlashCards.Helper;


namespace Buutyful.Coding_Tracker.State;

public class MainMenuState(StateManager stateManager) : IState
{
    private readonly StateManager _manager = stateManager;
    private static List<Commands> commands => new()
    {
         Commands.Info,
         Commands.View,
         Commands.Create,
         Commands.Clear,
         Commands.Quit
    };

    public ICommand GetCommand()
    {
        var command = UiHelper.DisplayOptions(commands.CommandsToStrings());
        return UiHelper.MenuSelector(command, _manager);
    }

    public void Render()
    {
        Console.Clear();
        Console.WriteLine("^^^^MainMenu^^^^");
    }

}
