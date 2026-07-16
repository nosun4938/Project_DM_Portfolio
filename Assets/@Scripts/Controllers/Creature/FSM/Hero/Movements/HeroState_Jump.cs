using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class HeroState_Jump : HeroStateBase
{
    public HeroState_Jump(Hero owner, HeroStateMachine stateMachine) : base(owner, stateMachine) { }
    float _fullJumpTime = 0.35f;
    float _timer = 0f;
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Jump;
        Owner.ModifyArmor(0);
        Owner.ModifyDamage(0);
        Owner.Rigidbody.gravityScale = 3.0f;

        _timer = 0f;
        Owner.HasJumped = true;
        Owner.Rigidbody.linearVelocityY = 60f;

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.None:
                Owner.Animator.Play("Jumping");
                break;
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Jumping");
                break;
        }
    }

    public override void Update()
    {
        base.Update();
        _timer += Time.deltaTime;

        // Idle
        if (Owner.IsGrounded && Mathf.Abs(Owner.Horizontal) < 0.01f)
        {
            _stateMachine.ChangeState(Owner._idleState);
            return;
        }

        // Run
        if (Owner.IsGrounded && Mathf.Abs(Owner.Horizontal) > 0.01f)
        {
            _stateMachine.ChangeState(Owner._runState);
            return;
        }

        // Fall
        if (Owner.Rigidbody.linearVelocityY < 0f)
        {
            _stateMachine.ChangeState(Owner._fallState);
                return;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if ((!Owner.IsJumpPressed || _timer > _fullJumpTime) && Owner.Rigidbody.linearVelocityY > 0f)
            Owner.Rigidbody.linearVelocityY *= 0.5f;
    }
}