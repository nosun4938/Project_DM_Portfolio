using UnityEngine;
using static Define;

public class BossState_Fall : BossStateBase
{
    public BossState_Fall(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Jump;
        Owner.Rigidbody.gravityScale = 10.0f;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        switch (Owner.CreatureWeapon)
        {
            default:
                Owner.Animator.Play("Metal_Falling");
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
    }

    public override void Exit()
    {
        base.Exit();
    }
}
