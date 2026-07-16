using System.Collections;
using UnityEngine;
using static Define;

public class BossState_Skill_Jump : BossStateBase
{
    public BossState_Skill_Jump(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Jump;

        Owner.Animator.Play("AirCrash_Pre", 0, 0f);
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(1.167f));

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(0.417f);
        Owner.Rigidbody.linearVelocityY = 80.0f;
        yield return new WaitForSeconds(delay - 0.417f - 0.333f);
        Owner.Rigidbody.linearVelocityY = 0f;
        yield return new WaitForSeconds(0.333f);
        NextSkillSlotPress();
    }
}
