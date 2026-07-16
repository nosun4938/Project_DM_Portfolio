using System.Collections;
using UnityEngine;
using static Define;

public class MonsterState_Stop : MonsterStateBase
{
    public MonsterState_Stop(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.Horizontal = 0f;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        Owner.Animator.Play("Metal_Run_Zend", 0, 0f);
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.083f));
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
        _stateMachine.ChangeState(Owner._engageState);
    }
}
