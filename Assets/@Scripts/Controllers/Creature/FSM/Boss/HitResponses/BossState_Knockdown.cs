using System.Collections;
using UnityEngine;
using static Define;

public class BossState_Knockdown : BossState_HitResponse
{
    public BossState_Knockdown(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Knockdown;
        Owner.Horizontal = 0f;

        // Knockdown delay: 1.167f (14/12)
        float delay = 1.167f;
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(delay));

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Knockdown", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_Knockdown", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;
            case ECreatureWeapon.BattleAxe:
                Owner.Animator.Play("Metal_Knockdown", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;

            default:
                Owner.Animator.Play("Metal_Knockdown", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;
        }
    }

    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._wakeUpState);
    }

    public override ESkillType GetNextHitResponse(SkillBase skill)
    {
        ESkillType skillType = skill.SkillType;

        // 기존 Switch문으로는 너무 보기 안좋아서 피격 관련은 신문법 사용.
        return skillType switch
        {
            ESkillType.Airborne => ESkillType.Airborne,
            ESkillType.Hitstun => ESkillType.LastHit,
            ESkillType.Stagger => ESkillType.LastHit,
            ESkillType.Knockdown => ESkillType.LastHit,
            _ => ESkillType.LastHit
        };
    }
}
