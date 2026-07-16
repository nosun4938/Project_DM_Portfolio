using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public abstract class CreatureStateBase
{
    protected Creature Owner;
    protected CreatureStateMachine _stateMachine;

    // 일단 Creature 별로 StateMachine 분리유지
    public virtual void Enter(Creature creature, CreatureStateMachine stateMachine)
    {
        Owner = creature;
        _stateMachine = stateMachine;
    }

    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}
