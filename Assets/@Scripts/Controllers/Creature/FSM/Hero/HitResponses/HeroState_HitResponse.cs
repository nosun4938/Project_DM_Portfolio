using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public abstract class HeroState_HitResponse : HeroStateBase
{
    public HeroState_HitResponse(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.ModifyArmor(0);
        Owner.ModifyDamage(0);
        Owner.NowKey = default;
        Owner.NowSkill = default;
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public abstract ESkillType GetNextHitResponse(SkillBase skill);
}
