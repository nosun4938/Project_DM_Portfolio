using System.Collections;
using UnityEngine;
using static Define;

public class BossState_StanceChange : BossStateBase
{
    public BossState_StanceChange(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.Horizontal = 0f;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);
        
        Owner.Animator.Play("Metal_StanceChange", 0, 0f);
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(1.750f));
    }

    public override void Update()
    {
        base.Update();

        if (Owner.IsGrounded == false)
        {
            _stateMachine.ChangeState(Owner._fallState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._engageState);
    }
}
