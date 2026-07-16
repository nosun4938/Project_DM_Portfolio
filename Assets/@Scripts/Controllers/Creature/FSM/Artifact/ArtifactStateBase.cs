using UnityEngine;

public abstract class ArtifactStateBase
{
    protected Artifact Owner;
    protected ArtifactStateMachine _stateMachine;
    protected ArtifactStateBase(Artifact owner, ArtifactStateMachine stateMachine)
    {
        Owner = owner;
        _stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
        Owner.StopCreatureCoroutine();
    }

    public virtual void Exit() { }
    public virtual void ReEnter()
    {
        Exit();
        Enter();
    }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
}
