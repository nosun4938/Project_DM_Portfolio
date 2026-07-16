using System.Collections;
using UnityEngine;
using static Define;

public class BossState_Skill_Mid : BossStateBase
{
    public BossState_Skill_Mid(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }

    bool nextPressed = false;
    public override void Enter()
    {
        base.Enter();
        nextPressed = false;
        Owner._lastSkillTime = Time.time;

        if (Owner.SkillHit)
        {
            // 같은 슬롯의 연계 스킬이 없는 경우, 다른 스킬 사용
            if (Owner.IsValidKey(Owner.NextSkillSlot) == false)
            {
                Owner.NextSkillSlot = Owner._bossAi.SkillHitSlot(Owner.NowKey.Slot);
            }
        }
        else
        {
            Owner.NextSkillSlot = Owner._bossAi.SkillMissSlot();
        }

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

        if (nextPressed == false && Owner.NextSkillSlot != ESkillSlot.None)
        {
            nextPressed = true;
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
