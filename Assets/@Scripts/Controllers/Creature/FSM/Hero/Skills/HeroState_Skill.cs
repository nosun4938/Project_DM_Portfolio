using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class HeroState_Skill : HeroStateBase
{
    public HeroState_Skill(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.StopResetKeyCoroutine();
        Owner.CreatureState = ECreatureState.Idle;

        if (Owner.Horizontal != 0)
            Owner.LookLeft = Owner.Horizontal > 0;

        Owner.SkillHit = false;
        Owner.SkillEnd = false;

        //Debug.Log($"Idle: Now Key {Owner.NowKey.Weapon}, {Owner.NowKey.Slot}, {Owner.NowKey.Combo}");
        Owner.Rigidbody.linearVelocityX = 0;
        Owner.Rigidbody.linearVelocityY = 0;

        Owner.NowSkill.DoSkill();
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        //base.FixedUpdate();
        if (Owner.Rigidbody.linearVelocityY < 0)
            Owner.Rigidbody.gravityScale = 15f;
    }

    public override void Exit()
    {
        //base.Exit();
        Owner.SkillEnd = true;
        Owner.ResetSkillKey();
    }
}
