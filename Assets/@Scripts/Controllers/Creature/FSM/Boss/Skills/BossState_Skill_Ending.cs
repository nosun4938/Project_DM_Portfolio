using System.Collections;
using UnityEngine;
using static Define;

public class BossState_Skill_Ending : BossStateBase
{
    public BossState_Skill_Ending(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        Owner.StartCreatureCoroutine(EndSkillAfterDelay(Owner.SkillEndLength));
        Owner.NextSkillSlot = ESkillSlot.None;
        Owner.NowKey = default;
    }

    public override void Exit()
    {
        base.Exit();
        Owner.StopCreatureCoroutine();
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._engageState);
    }
}
