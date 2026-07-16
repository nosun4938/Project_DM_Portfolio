using System.Collections;
using UnityEngine;
using static Define;

public class HeroState_Death : HeroStateBase
{
    public HeroState_Death(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Dead;

        Owner.Rigidbody.linearVelocityX = 0;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);
        Owner.HitCircle.gameObject.SetActive(false);

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Death", 0, 0f);
                break;
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_Death", 0, 0f);
                break;
            case ECreatureWeapon.BattleAxe:
                Owner.Animator.Play("Metal_Death", 0, 0f);
                break;
        }
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(2.25f));
    }

    public override void FixedUpdate()
    {
        //base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Owner.Respawn();
    }
}
