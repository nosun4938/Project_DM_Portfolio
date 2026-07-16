using System.Collections;
using UnityEngine;
using static Define;

public class ArtifactState_Death : ArtifactStateBase
{
    public ArtifactState_Death(Artifact owner, ArtifactStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Dead;

        Owner.ModifyArmor(0);
        Owner.HitCircle.size = new Vector2(0, 0);

        Owner.Animator.Play("Armor_Death", 0, 0f);
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.500f));
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Owner.gameObject.SetActive(false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
