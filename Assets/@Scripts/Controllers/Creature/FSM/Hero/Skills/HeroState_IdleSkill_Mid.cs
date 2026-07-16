using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class HeroState_IdleSkill_Mid : HeroStateBase
{
    public HeroState_IdleSkill_Mid(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner._lastSkillTime = Time.time;

        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.2f));
    }

    public override void Exit()
    {
        base.Exit();
        Owner.SkillHit = false;
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
        _stateMachine.ChangeState(Owner._skillEndingState);
    }
}
