using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class ArtifactState_Idle : ArtifactStateBase
{
    public ArtifactState_Idle(Artifact owner, ArtifactStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();

        Owner.CreatureState = ECreatureState.Idle;

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Barrier:
                Owner.Animator.Play("Armor_Idle", 0, 0f);
                break;
            default:
                Owner.Animator.Play("No_Idle", 0, 0f);
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        if (Owner.ArtifactCounter == 0 && Owner.CreatureSuperArmor > 0)
        {
            _stateMachine.ChangeState(Owner._skillState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
