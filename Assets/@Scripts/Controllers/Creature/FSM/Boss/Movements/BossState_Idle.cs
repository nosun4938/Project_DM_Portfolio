using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static Define;

public class BossState_Idle : BossStateBase
{
    public BossState_Idle(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    float _waitTime = 1f;
    float _timer = 0f;
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        _waitTime = Random.Range(0.5f, 1.5f);
        _timer = 0f;
        Owner.Horizontal = 0f;

        switch (Owner.CreatureWeapon)
        {
            default:
                Owner.Animator.Play("Metal_Idle", 0, 0f);
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        if (Owner.IsAggro)
        {
            _stateMachine.ChangeState(Owner._changeState);
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _waitTime)
        {
            _stateMachine.ChangeState(Owner._patrolState);
            return;
        }

        if (Owner.IsGrounded == false)
        {
            _stateMachine.ChangeState(Owner._fallState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _timer = 0f;
    }
}
