using System.Collections;
using UnityEngine;
using static Define;

public class BossState_Chase : BossStateBase
{
    public BossState_Chase(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    ESkillSlot _skillSlot;
    float distance;
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.Horizontal = 0f;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        Owner.Animator.Play("Metal_Run_Astart", 0, 0f);
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.417f));

        Owner.Horizontal = Owner.TargetDistance < 0 ? 1 : -1;
        Owner.Animator.Play("Metal_Run_Mid", 0, 0f);

        float dice = Random.Range(0, 10);
        float absDistance = Mathf.Abs(Owner.TargetDistance);

        if (absDistance < 52)
        {
            if (dice < 5)
                distance = 11f;
            else
                distance = 19f;
        }
        else
        {
            if (dice < 3)
                distance = 11f;
            else if (dice < 6)
                distance = 16f;
            else
                distance = 55f;
        }
    }

    public override void Update()
    {
        base.Update();

        if (Owner.IsGrounded == false)
        {
            _stateMachine.ChangeState(Owner._fallState);
            return;
        }

        if (Mathf.Abs(Owner.TargetDistance) < distance)
        {
            _skillSlot = Owner._bossAi.DistanceSlot();

            if (_skillSlot != ESkillSlot.None)
            {
                Owner.NextSkillSlot = _skillSlot;
                NextSkillSlotPress();
                return;
            }

            _stateMachine.ChangeState(Owner._engageState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        Owner.Horizontal = 0;
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}