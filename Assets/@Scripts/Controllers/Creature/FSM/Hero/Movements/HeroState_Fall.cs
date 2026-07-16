using UnityEngine;
using static Define;
using UnityEngine.InputSystem;

public class HeroState_Fall : HeroStateBase
{
    public HeroState_Fall(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Jump;
        Owner.Rigidbody.gravityScale = 15.0f;

        Owner.ModifyArmor(0);
        Owner.ModifyDamage(0);

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_Falling");
                break;
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Falling");
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        // Idle
        if (Owner.IsGrounded && Mathf.Abs(Owner.Horizontal) < 0.01f)
        {
            _stateMachine.ChangeState(Owner._landState);
            return;
        }

        // Run
        if (Owner.IsGrounded && Mathf.Abs(Owner.Horizontal) > 0.01f)
        {
            _stateMachine.ChangeState(Owner._runState);
            return;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
