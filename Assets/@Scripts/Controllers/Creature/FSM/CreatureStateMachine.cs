using UnityEngine;

public class CreatureStateMachine
{
    private Creature Owner;
    private CreatureStateBase _currentState;

    public bool _isChangingState = false;

    public CreatureStateMachine(Creature creature)
    {
        Owner = creature;
    }

    public void ChangeState(CreatureStateBase newState)
    {
        //Debug.Log($"ChangeState: {_currentState?.GetType().Name} ¡æ {newState?.GetType().Name}");
        if (_currentState == newState)
            return;

        if (_isChangingState)
            return;

        _isChangingState = true;

        _currentState?.Exit();
        _currentState = newState;
        _currentState?.Enter(Owner, this);

        _isChangingState = false;
    }

    public void Update() => _currentState?.Update();
    public void FixedUpdate() => _currentState?.FixedUpdate();
}
