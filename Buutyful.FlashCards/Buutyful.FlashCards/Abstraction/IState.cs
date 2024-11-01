namespace Buutyful.Coding_Tracker.Abstraction;

public interface IState
{
    ICommand GetCommand();
    void Render();
}
