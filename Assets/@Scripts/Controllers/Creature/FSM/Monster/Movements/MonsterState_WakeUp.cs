using System.Collections;
using UnityEngine;
using static Define;

public class MonsterState_WakeUp : MonsterStateBase
{
    public MonsterState_WakeUp(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();

        Owner.CreatureState = ECreatureState.Idle;
        Owner.Horizontal = 0f;
        Owner.ModifyArmor(100);

        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.333f));
        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_WakeUp", 0, 0f);
                break;
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_WakeUp", 0, 0f);
                break;

            default:
                Owner.Animator.Play("No_WakeUp", 0, 0f);
                break;
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        Owner.ModifyArmor(0);
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._idleState);
    }
}
