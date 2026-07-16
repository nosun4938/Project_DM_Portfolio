using UnityEngine;
using UnityEngine.InputSystem;

public class BossStateMachine
{
    private Boss Owner;
    private BossStateBase _currentState;
    public BossStateBase CurrentState { get { return _currentState; } }

    public BossStateMachine(Boss boss)
    {
        Owner = boss;
    }

    public void ChangeState(BossStateBase newState)
    {
        //Debug.Log($"ChangeState: {_currentState?.GetType().Name} => {newState?.GetType().Name}");
        if (_currentState == newState)
        {
            _currentState.ReEnter();
            return;
        }

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter();
    }

    public void Update() => _currentState?.Update();
    public void FixedUpdate() => _currentState?.FixedUpdate();
}
