using UnityEngine;
using static Define;

public class MonsterState_Idle : MonsterStateBase
{
    public MonsterState_Idle(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    float _waitTime;
    float _timer = 0f;
    public override void Enter()
    {
        base.Enter();

        Owner.CreatureState = ECreatureState.Idle;
        _waitTime = Random.Range(0.5f, 1.5f);
        _timer = 0f;
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
            default:
                Owner.Animator.Play("No_Idle", 0, 0f);
                break;
        }
    }

    public override void Update()
    {
        if (Owner.IsAggro && Owner.CreatureWeapon != ECreatureWeapon.None)
        {
            _stateMachine.ChangeState(Owner._engageState);
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _waitTime && Owner.CreatureWeapon != ECreatureWeapon.None)
        {
            _stateMachine.ChangeState(Owner._patrolState);
            return;
        }

        if (Owner.IsGrounded == false && Owner.CreatureWeapon != ECreatureWeapon.None)
        {
            _stateMachine.ChangeState(Owner._fallState);
            return;
        }
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        _timer = 0f;
    }
}
