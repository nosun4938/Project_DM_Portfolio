using UnityEngine;

public class MonsterState_Skill : MonsterStateBase
{
    public MonsterState_Skill(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.SkillHit = false;
        Owner.SkillEnd = false;

        FaceToTarget();
        Owner.StopResetKeyCoroutine();

        Owner.NowSkill.DoSkill();
        Owner.Rigidbody.linearVelocityX = 0;
        Owner.Horizontal = 0;
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        Owner.SkillEnd = true;
        Owner.ResetSkillKey();
    }
}
