using UnityEngine;
using UnityEngine.InputSystem;

public class HeroStateMachine
{
    private Hero Owner;
    private HeroStateBase _currentState;
    public HeroStateBase CurrentState { get { return _currentState; } }
    public HeroStateMachine(Hero hero)
    {
        Owner = hero;
    }

    public void ChangeState(HeroStateBase newState)
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

    public void OnMove(InputAction.CallbackContext context) => _currentState?.OnMove(context);
    public void OnJump(InputAction.CallbackContext context) => _currentState?.OnJump(context);
    public void OnZSkill(InputAction.CallbackContext context) => _currentState.OnZSkill(context);
    public void OnXSkill(InputAction.CallbackContext context) => _currentState.OnXSkill(context);
    public void OnASkill(InputAction.CallbackContext context) => _currentState?.OnASkill(context);
    public void OnSSkill(InputAction.CallbackContext context) => _currentState?.OnSSkill(context);
    public void OnDSkill(InputAction.CallbackContext context) => _currentState?.OnDSkill(context);
    public void OnFSkill(InputAction.CallbackContext context) => _currentState?.OnFSkill(context);
    public void OnDash(InputAction.CallbackContext context) => _currentState.OnDash(context);
}
