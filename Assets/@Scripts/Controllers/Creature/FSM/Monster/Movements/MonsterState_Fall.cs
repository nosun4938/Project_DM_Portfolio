using UnityEngine;
using static Define;

public class MonsterState_Fall : MonsterStateBase
{
    public MonsterState_Fall(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Jump;
        Owner.Rigidbody.gravityScale = 20.0f;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

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
    }

    public override void Exit()
    {
        base.Exit();
        Owner.Rigidbody.gravityScale = 5.0f;
    }
}
