using System.Collections;
using UnityEngine;
using static Define;

public class MonsterState_Land : MonsterStateBase
{
    public MonsterState_Land(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_Landing");
                break;
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Landing");
                break;
        }
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.333f));
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
