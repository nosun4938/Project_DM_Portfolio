using System.Collections;
using UnityEngine;
using static Define;

public abstract class MonsterStateBase
{
    protected Monster Owner { get; private set; }
    protected MonsterStateMachine _stateMachine;

    protected MonsterStateBase(Monster owner, MonsterStateMachine stateMachine)
    {
        Owner = owner;
        _stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        Owner.StopCreatureCoroutine();
    }
    public virtual void Exit()
    {
        Owner.Rigidbody.gravityScale = 3.0f;
    }
    public virtual void Update()
    {
        if (Mathf.Abs(Owner.TargetDistance) < 20f && Mathf.Abs(Owner.TargetDistanceY) < 20f)
            Owner.IsAggro = true;

        if (Mathf.Abs(Owner.TargetDistance) > 40f || Mathf.Abs(Owner.TargetDistanceY) > 32f)
            Owner.IsAggro = false;
    }
    public virtual void FixedUpdate()
    {
        ApplyHorizontalMovement();
    }

    public virtual void ReEnter()
    {
        Exit();
        Enter();
    }

    #region InputSystem
    // Movements
    public virtual void OnJump()
    {

    }
    public virtual void OnMove()
    {

    }

    // Skills
    public virtual void OnShiftSkill()
    {
        Owner.InputChange(ESkillSlot.Shift);
        return;
    }
    public virtual void OnXSkill()
    {
        Owner.InputChange(ESkillSlot.X);
        return;
    }

    public virtual void OnZSkill()
    {
        Owner.InputChange(ESkillSlot.Z);
        return;
    }
    public virtual void OnASkill()
    {
        Owner.InputChange(ESkillSlot.A);
        return;
    }
    public virtual void OnSSkill()
    {
        Owner.InputChange(ESkillSlot.S);
        return;
    }
    public virtual void OnDSkill()
    {
        Owner.InputChange(ESkillSlot.D);
        return;
    }
    public virtual void OnFSkill()
    {
        Owner.InputChange(ESkillSlot.F);
        return;
    }

    #endregion

    #region Util
    protected void ApplyHorizontalMovement()
    {
        float targetSpeed = Owner.Horizontal * Owner.MoveSpeed;
        Owner.Rigidbody.linearVelocityX = targetSpeed;

        if (Owner.Horizontal != 0)
            Owner.LookLeft = Owner.Horizontal > 0;
    }

    protected void FaceToTarget()
    {
        if (Owner.TargetDistance > 0)
            Owner.Horizontal = -1;
        else if (Owner.TargetDistance < 0)
            Owner.Horizontal = 1;

        Owner.LookLeft = Owner.Horizontal > 0;
    }

    // NextSkillSlot에 저장된 스킬 입력
    protected void NextSkillSlotPress()
    {
        if (Owner.NextSkillSlot != ESkillSlot.None)
        {
            ESkillSlot skillSlot = Owner.NextSkillSlot;
            SkillSlotPress(skillSlot);
        }
    }

    // 직접 스킬 키 입력
    protected void SkillSlotPress(ESkillSlot skillSlot)
    {
        switch (skillSlot)
        {
            case ESkillSlot.Shift:
                OnShiftSkill();
                break;
            case ESkillSlot.Z:
                OnZSkill();
                break;
            case ESkillSlot.X:
                OnXSkill();
                break;
            case ESkillSlot.A:
                OnASkill();
                break;
            case ESkillSlot.S:
                OnSSkill();
                break;
            case ESkillSlot.D:
                OnDSkill();
                break;
            case ESkillSlot.F:
                OnFSkill();
                break;
            default:
                break;
        }
    }
    #endregion
}
