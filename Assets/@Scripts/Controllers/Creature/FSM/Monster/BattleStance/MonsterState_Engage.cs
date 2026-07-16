using UnityEngine;
using static Define;

public class MonsterState_Engage : MonsterStateBase
{
    public MonsterState_Engage(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }

    float _waitTime = 1f;
    float _timer = 0f;

    public override void Enter()
    {
        base.Enter();
        FaceToTarget();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.Horizontal = 0f;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Idle", 0, 0f);
                break;
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_Idle", 0, 0f);
                break;
        }

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        _waitTime = Random.Range(0.2f, 0.5f);
    }

    public override void Update()
    {
        base.Update();

        if (Owner.IsAggro == false)
        {
            _stateMachine.ChangeState(Owner._idleState);
            return;
        }

        if (Owner.IsGrounded == false)
        {
            _stateMachine.ChangeState(Owner._fallState);
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _waitTime)
        {
            _stateMachine.ChangeState(Owner._chaseState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _timer = 0f;
    }
}
