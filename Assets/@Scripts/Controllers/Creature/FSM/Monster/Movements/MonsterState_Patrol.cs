using UnityEngine;
using static Define;

public class MonsterState_Patrol : MonsterStateBase
{
    public MonsterState_Patrol(Monster owner, MonsterStateMachine stateMachine) : base(owner, stateMachine) { }
    float _waitTime;
    float _timer = 0f;
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        _waitTime = Random.Range(0.5f, 3.0f);
        _timer = 0f;

        float horizontalDir = Owner.InitVector.x - Owner.transform.position.x;
        Owner.Horizontal = horizontalDir > 0 ? 1 : -1;

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                Owner.Animator.Play("Wood_Run_Mid", 0, 0f);
                break;
            case ECreatureWeapon.Dagger:
                Owner.Animator.Play("Fire_Run_Mid", 0, 0f);
                break;
        }
    }

    public override void Update()
    {
        base.Update();

        if (Owner.IsAggro)
        {
            _stateMachine.ChangeState(Owner._engageState);
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _waitTime)
        {
            _stateMachine.ChangeState(Owner._idleState);
            return;
        }

        if (Owner.IsGrounded == false)
        {
            //_stateMachine.ChangeState(Owner._fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
