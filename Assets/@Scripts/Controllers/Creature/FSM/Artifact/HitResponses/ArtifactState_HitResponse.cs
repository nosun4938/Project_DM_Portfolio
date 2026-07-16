using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class ArtifactState_HitResponse : ArtifactStateBase
{
    public ArtifactState_HitResponse(Artifact owner, ArtifactStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();

        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.250f));
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._idleState);
    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        base.Update();
    }
}
