using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class HeroState_WakeUp : HeroStateBase
{
    public HeroState_WakeUp(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.ModifyArmor(100);
        Owner.CreatureState = ECreatureState.Idle;

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_WakeUp");
                break;
        }
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.417f));
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
        _stateMachine.ChangeState(Owner._idleState);
    }
}
