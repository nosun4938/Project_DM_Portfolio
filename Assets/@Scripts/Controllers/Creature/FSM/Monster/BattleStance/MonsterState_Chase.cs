using System.Collections;
using UnityEngine;
using static Define;

public class MonsterState_Chase : MonsterStateBase
{
    public MonsterState_Chase(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    ESkillSlot _skillSlot;
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.Horizontal = 0f;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        Owner.Horizontal = Owner.TargetDistance < 0 ? 1 : -1;
        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Run_Mid", 0, 0f);
                break;
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_Run_Mid", 0, 0f);
                break;
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

        if (Mathf.Abs(Owner.TargetDistance) < 8f)
        {
            _skillSlot = ESkillSlot.Z;
            if (_skillSlot != ESkillSlot.None)
            {
                Owner.NextSkillSlot = _skillSlot;
                NextSkillSlotPress();
                return;
            }
        }
        else if (Mathf.Abs(Owner.TargetDistance) < 15f)
        {
            _skillSlot = ESkillSlot.X;
            if (_skillSlot != ESkillSlot.None)
            {
                Owner.NextSkillSlot = _skillSlot;
                NextSkillSlotPress();
                return;
            }
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
