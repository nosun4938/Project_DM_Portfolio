using System.Collections;
using UnityEngine;
using static Define;

public class BossState_Stagger : BossState_HitResponse
{
    public BossState_Stagger(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    float decel;
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Stagger;
        Owner.Horizontal = 0f;

        // Stagger delay: 0.833f (10/12)
        float delay = 0.833f;
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(delay));

        // 피격 이동
        float hitVelocity = 20 * (1f + 0.5f * Owner.IsCreatureShoved);
        if (Owner.LookLeft)
            Owner.Rigidbody.linearVelocityX = -hitVelocity;
        else
            Owner.Rigidbody.linearVelocityX = hitVelocity;

        // Decel 계산
        decel = hitVelocity / (delay * 0.7f);

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Stagger", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_Stagger", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;
            case ECreatureWeapon.BattleAxe:
                Owner.Animator.Play("Metal_Stagger", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;

            default:
                Owner.Animator.Play("Metal_Stagger", 0, 0f);
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
        Owner.Rigidbody.linearVelocityX = Mathf.MoveTowards(Owner.Rigidbody.linearVelocityX, 0f, decel * Time.deltaTime);
    }
    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _stateMachine.ChangeState(Owner._engageState);
    }

    public override ESkillType GetNextHitResponse(SkillBase skill)
    {
        ESkillType skillType = skill.SkillType;

        // 기존 Switch문으로는 너무 보기 안좋아서 피격 관련은 신문법 사용.
        return skillType switch
        {
            ESkillType.Airborne => ESkillType.Airborne,
            ESkillType.Hitstun => ESkillType.Knockdown,
            ESkillType.Stagger => ESkillType.Knockdown,
            ESkillType.Knockdown => ESkillType.Knockdown,
            _ => skillType
        };
    }
}
