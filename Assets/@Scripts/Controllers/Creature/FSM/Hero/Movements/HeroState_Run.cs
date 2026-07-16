using UnityEngine;
using static Define;
using UnityEngine.InputSystem;

public class HeroState_Run : HeroStateBase
{
    public HeroState_Run(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.ModifyArmor(0);
        Owner.ModifyDamage(0);

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.None:
                Owner.Animator.Play("Run_Mid");
                break;
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Run_Mid");
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        // Idle
        if (Mathf.Abs(Owner.Horizontal) < 0.01f)
        {
            _stateMachine.ChangeState(Owner._idleState);
            return;
        }

        // Jump
        if (Owner.CoyoteTimeCounter < Owner.CoyoteTime && Owner.Rigidbody.linearVelocityY > 0f)
        {
            _stateMachine.ChangeState(Owner._jumpState);
            return;
        }

        // Fall
        if (Owner.CoyoteTimeCounter < Owner.CoyoteTime && Owner.Rigidbody.linearVelocityY < 0f)
        {
            _stateMachine.ChangeState(Owner._fallState);
            return;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    #region InputSystem
    public override void OnMove(InputAction.CallbackContext context)
    {
        base.OnMove(context);
    }
    public override void OnJump(InputAction.CallbackContext context)
    {
        base.OnJump(context);
    }
    public override void OnDash(InputAction.CallbackContext context)
    {
        base.OnDash(context);
    }

    // Skills
    public override void OnZSkill(InputAction.CallbackContext context)
    {
        base.OnZSkill(context);
    }
    public override void OnASkill(InputAction.CallbackContext context)
    {
        base.OnASkill(context);
    }
    public override void OnSSkill(InputAction.CallbackContext context)
    {
        base.OnSSkill(context);
    }
    public override void OnDSkill(InputAction.CallbackContext context)
    {
        base.OnDSkill(context);
    }
    public override void OnFSkill(InputAction.CallbackContext context)
    {
        base.OnFSkill(context);
    }
    #endregion
}
