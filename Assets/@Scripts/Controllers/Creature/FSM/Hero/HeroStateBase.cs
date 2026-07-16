using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public abstract class HeroStateBase
{
    protected Hero Owner;
    protected HeroStateMachine _stateMachine;
    protected HeroStateBase(Hero owner, HeroStateMachine stateMachine)
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
        if (!Owner.SkillEnd)
            return;

        HandleCoyoteTime();
    }
    public virtual void FixedUpdate()
    {
        if (!Owner.SkillEnd)
            return;

        ApplyHorizontalMovement();
    }
    public virtual void ReEnter()
    {
        Exit();
        Enter();
    }

    #region InputSystem
    // Movements
    public void OnSlotInput(InputAction.CallbackContext context, ESkillSlot slot)
    {
        if (context.performed)
        {
            Owner._inputPressed[slot] = true;
            Owner._lastInputTime[slot] = Time.time;
            Owner.BufferInput(slot);
        }

        if (context.canceled)
            Owner._inputPressed[slot] = false;
    }

    public virtual void OnJump(InputAction.CallbackContext context)
    {
        if (Managers.Game.GameState != EGameState.Playing)
            return;

        Owner.IsJumpPressed = context.ReadValue<float>() > 0.5f;

        if (context.performed)
        {
            Owner.BufferInput(ESkillSlot.C);
        }
    }
    public virtual void OnMove(InputAction.CallbackContext context)
    {
        if (Managers.Game.GameState != EGameState.Playing)
            return;

        if (context.performed)
            Owner.Horizontal = context.ReadValue<Vector2>().x;

        if (context.canceled)
            Owner.Horizontal = 0;
    }
    public virtual void OnDash(InputAction.CallbackContext context)
    {
        if (Managers.Game.GameState != EGameState.Playing)
            return;

        OnSlotInput(context, ESkillSlot.Shift);
    }

    // Skills
    public virtual void OnXSkill(InputAction.CallbackContext context)
    {
        if (Managers.Game.GameState != EGameState.Playing)
            return;

        OnSlotInput(context, ESkillSlot.X);
    }

    public virtual void OnZSkill(InputAction.CallbackContext context)
    {
        if (Managers.Game.GameState != EGameState.Playing)
            return;

        OnSlotInput(context, ESkillSlot.Z);
    }
    public virtual void OnASkill(InputAction.CallbackContext context)
    {
        if (Managers.Game.GameState != EGameState.Playing)
            return;

        OnSlotInput(context, ESkillSlot.A);
    }
    public virtual void OnSSkill(InputAction.CallbackContext context)
    {
        if (Managers.Game.GameState != EGameState.Playing)
            return;

        OnSlotInput(context, ESkillSlot.S);
    }
    public virtual void OnDSkill(InputAction.CallbackContext context)
    {
        if (Managers.Game.GameState != EGameState.Playing)
            return;

        if (context.performed)
        {
            Owner.BufferInput(ESkillSlot.D);
        }
    }
    public virtual void OnFSkill(InputAction.CallbackContext context)
    {
        if (Managers.Game.GameState != EGameState.Playing)
            return;

        if (context.performed)
        {
            Owner.BufferInput(ESkillSlot.F);
        }
    }

    #endregion

    #region Physics Handler
    protected void HandleCoyoteTime()
    {
        if (Owner.IsGrounded)
        {
            Owner.CoyoteTimeCounter = Owner.CoyoteTime;
            Owner.HasJumped = false;
        }
        else
        {
            Owner.CoyoteTimeCounter -= Time.deltaTime;
        }
    }

    protected void ApplyHorizontalMovement()
    {
        float targetSpeed = Owner.Horizontal * Owner.MoveSpeed;
        Owner.Rigidbody.linearVelocityX = targetSpeed;

        if (Owner.Horizontal != 0)
            Owner.LookLeft = Owner.Horizontal > 0;
    }
    #endregion
}