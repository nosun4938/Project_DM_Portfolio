using System.Collections;
using UnityEngine;
using static Define;

public class ArtifactState_Break : ArtifactStateBase
{
    public ArtifactState_Break(Artifact owner, ArtifactStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Barrier:
                Owner.Animator.Play("Armor_Hited", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;

            default:
                Owner.Animator.Play("No_Hited", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;
        }
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.417f));
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._idleState);
    }

    public override void Exit()
    {
        base.Update();
    }

    public override void Update()
    {
        base.Update();
    }
}
