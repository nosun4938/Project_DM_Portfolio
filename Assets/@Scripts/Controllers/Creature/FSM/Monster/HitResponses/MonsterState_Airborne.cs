using System.Collections;
using UnityEngine;
using static Define;

public class MonsterState_Airborne : MonsterState_HitResponse
{
    public MonsterState_Airborne(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    float decel;
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Airborne;
        Owner.Horizontal = 0f;

        // Airborne delay 1.0f (15/12)
        float delay = 1.25f;
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(delay));

        // 피격 이동
        Owner.Rigidbody.linearVelocityY = CalculateAirborneVelocityY(delay);
        float hitVelocity =  10f + 5f * Owner.IsCreatureShoved;
        if (Owner.LookLeft)
            Owner.Rigidbody.linearVelocityX = -hitVelocity;
        else
            Owner.Rigidbody.linearVelocityX = hitVelocity;

        // Decel 계산
        decel = hitVelocity / (delay * 1.0f);

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Airborne", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_Airborne", 0, 0f);
                Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
                break;

            default:
                Owner.Animator.Play("No_Airborne", 0, 0f);
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
        _stateMachine.ChangeState(Owner._knockdownState);
    }

    public override ESkillType GetNextHitResponse(SkillBase skill)
    {
        ESkillType skillType = skill.SkillType;

        // 기존 Switch문으로는 너무 보기 안좋아서 피격 관련은 신문법 사용.
        return skillType switch
        {
            ESkillType.Airborne => ESkillType.Airborne,
            ESkillType.Hitstun => ESkillType.Airborne,
            ESkillType.Stagger => ESkillType.Airborne,
            ESkillType.Knockdown => ESkillType.Airborne,
            _ => ESkillType.Airborne
        };
    }

    public float CalculateAirborneVelocityY(float targetAirTime)
    {
        Rigidbody2D rb = Owner.Rigidbody;

        float currentY = Owner.transform.position.y;
        float groundY = Owner.LastPosition.y;

        float g = Physics2D.gravity.y * rb.gravityScale;
        float t = targetAirTime;
        float v0 = (groundY - currentY - 0.5f * g * t * t) / t;

        return v0;
    }
}
