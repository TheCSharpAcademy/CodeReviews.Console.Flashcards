using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.Coding_Tracker.State;
using Buutyful.FlashCards.Helper;


namespace Buutyful.FlashCards.State;

public class ViewState(StateManager manager) : IState
{
    private readonly StateManager _stateManager = manager;   
    private readonly List<Commands> commands = new()
    {        
        Commands.Decks,
        Commands.Cards,
        Commands.Sessions,
        Commands.Back,
        Commands.Forward,
        Commands.Quit
    };
    public ICommand GetCommand()
    {
        var command = UiHelper.DisplayOptions(commands.CommandsToStrings());
        return UiHelper.MenuSelector(command, _stateManager);
    }

    public void Render()
    {
        Console.Clear();
        Console.WriteLine("What do you want to see?");
    }
}
