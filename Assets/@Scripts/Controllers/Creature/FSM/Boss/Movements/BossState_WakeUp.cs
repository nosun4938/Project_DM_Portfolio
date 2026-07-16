using System.Collections;
using UnityEngine;
using static Define;

public class BossState_WakeUp : BossStateBase
{
    public BossState_WakeUp(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.ModifyArmor(100);

        switch (Owner.CreatureWeapon)
        {
            default:
                Owner.Animator.Play("Metal_WakeUp", 0, 0f);
                break;
        }

        ESkillSlot _skillSlot = Owner._bossAi.DistanceSlot();
        Owner.NextSkillSlot = _skillSlot;

        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.750f));
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        //base.FixedUpdate();
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SkillSlotPress(ESkillSlot.X);
    }
    public override void Exit()
    {
        base.Exit();
        Owner.ModifyArmor(0);
    }
}
