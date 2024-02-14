using Buutyful.Coding_Tracker.Abstraction;
using Buutyful.FlashCards.Helper;


namespace Buutyful.Coding_Tracker.State;

public class MainMenuState(StateManager stateManager) : IState
{
    private readonly StateManager _manager = stateManager;

    public ICommand GetCommand()
    {
        var command = UiHelper.DisplayOptions().ToLower();
        return UiHelper.MenuSelector(command, _manager);
    }

    public void Render()
    {       
    }
 
}
