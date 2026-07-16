using System.Collections;
using UnityEngine;
using static Define;

public class MonsterState_Skill_Mid : MonsterStateBase
{
    public MonsterState_Skill_Mid(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner._lastSkillTime = Time.time;

        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.2f));
    }

    public override void Exit()
    {
        base.Exit();
        Owner.StopCreatureCoroutine();
        Owner.SkillHit = false;
    }

    public override void Update()
    {
        base.Update();

        if (Owner.NextSkillSlot != ESkillSlot.None)
        {
            NextSkillSlotPress();
            return;
        }
    }
    public override void FixedUpdate()
    {
        //base.FixedUpdate();
    }
    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._skillEndingState);
    }
}
