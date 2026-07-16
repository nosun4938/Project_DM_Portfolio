using System.Collections;
using UnityEngine;
using static Define;

public class BossState_Land : BossStateBase
{
    public BossState_Land(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        switch (Owner.CreatureWeapon)
        {
            default:
                Owner.Animator.Play("Metal_Landing");
                break;
        }
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.833f));
    }

    public override void Update()
    {
        base.Update();

        if (Owner.IsGrounded == false)
        {
            _stateMachine.ChangeState(Owner._fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._idleState);
    }
}
