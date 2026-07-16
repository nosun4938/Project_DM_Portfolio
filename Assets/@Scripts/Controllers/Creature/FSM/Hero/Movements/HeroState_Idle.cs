using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class HeroState_Idle : HeroStateBase
{
    public HeroState_Idle(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.ModifyArmor(0);
        Owner.ModifyDamage(0);

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.None:
                Owner.Animator.Play("Idle");
                break;
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Idle");
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        // Run
        if (Mathf.Abs(Owner.Horizontal) > 0.01f)
        {
            _stateMachine.ChangeState(Owner._runState);
            return;
        }

        // Jump
        if (Owner.CoyoteTimeCounter < Owner.CoyoteTime)
        {
            _stateMachine.ChangeState(Owner._jumpState);
            return;
        }

        // Fall
        if (Owner.CoyoteTimeCounter < Owner.CoyoteTime && Owner.Rigidbody.linearVelocityY <= 0f)
        {
            _stateMachine.ChangeState(Owner._fallState);
            return;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
