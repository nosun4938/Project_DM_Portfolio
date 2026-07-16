using System.Collections;
using UnityEngine;
using static Define;

public class BossState_Death : BossStateBase
{
    public BossState_Death(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Dead;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);
        Owner.HitCircle.size = new Vector2(0, 0);

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
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(2.5f));
    }

    public override void Exit()
    {
        base.Exit();
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //Owner.gameObject.SetActive(false);
    }
}
