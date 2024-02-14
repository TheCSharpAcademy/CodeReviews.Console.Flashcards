using Buutyful.Coding_Tracker.Abstraction;


namespace Buutyful.Coding_Tracker.State;

public class StateManager
{
    private readonly Stack<IState> _back = new();
    private readonly Queue<IState> _forward = new();
    private IState _currentState;
    public void SwitchState(IState state)
    {
        _back.Push(_currentState);
        _currentState = state;
    }
    public IState PastState()
    {
        if (_back.Count > 0)
        {
            var past = _back.Pop();
            _forward.Enqueue(past);
            return past;
        }
        return _currentState;
    }
    public IState FutureState()
    {
        if (_forward.Count > 0)
        {
            var future = _forward.Dequeue();
            _back.Push(future);
            return future;
        }
        return _currentState;
    }
    public void Run(IState initialState)
    {
        _currentState = initialState;
        while (true)
        {
            _currentState.Render();
            var command = _currentState.GetCommand();
            command.Execute();
        }
    }
}
