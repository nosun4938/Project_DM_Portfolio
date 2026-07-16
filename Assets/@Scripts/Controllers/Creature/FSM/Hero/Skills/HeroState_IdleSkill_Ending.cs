using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class HeroState_IdleSkill_Ending : HeroStateBase
{
    public HeroState_IdleSkill_Ending(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        Owner.StartCreatureCoroutine(EndSkillAfterDelay(Owner.SkillEndLength));
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
        // Run
        if (Mathf.Abs(Owner.Horizontal) > 0.01f)
        {
            _stateMachine.ChangeState(Owner._runState);
            return;
        }

        // Jump
        if (Owner.CoyoteTimeCounter < Owner.CoyoteTime)
        {
            _stateMachine.ChangeState(Owner._jumpState);
            return;
        }

        // Fall
        if (Owner.CoyoteTimeCounter < Owner.CoyoteTime && Owner.Rigidbody.linearVelocityY <= 0f)
        {
            _stateMachine.ChangeState(Owner._fallState);
            return;
        }
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._idleState);
    }
}