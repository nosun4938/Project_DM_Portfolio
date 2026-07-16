using UnityEngine;

public class ArtifactState_Skill : ArtifactStateBase
{
    public ArtifactState_Skill(Artifact owner, ArtifactStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.NowSkill.DoSkill();
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
        Owner.ModifyArmor(5);
        Owner.ModifyCounter(3);
    }
}
