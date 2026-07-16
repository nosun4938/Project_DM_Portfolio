using System.Collections;
using UnityEngine;
using static Define;

public class BossState_Patrol : BossStateBase
{
    public BossState_Patrol(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    float _waitTime = 1f;
    float _timer = 0f;
    public override void Enter()
    {
        base.Enter();
        Owner.CreatureState = ECreatureState.Idle;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        _waitTime = Random.Range(0.5f, 5.0f);
        _timer = 0f;

        float horizontalDir = Owner.InitVector.x - Owner.transform.position.x;
        Owner.Horizontal = horizontalDir > 0 ? 1 : -1;

        Owner.Animator.Play("Metal_Walk_Astart");
        Owner.StartCreatureCoroutine(EndSkillAfterDelay(0.167f));

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.BattleAxe:
                Owner.Animator.Play("Metal_Walk_Mid");
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
            _stateMachine.ChangeState(Owner._idleState);
            return;
        }

        if (Owner.IsGrounded == false)
        {
            _stateMachine.ChangeState(Owner._fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        _timer = 0f;
        Owner.Horizontal = 0;
    }

    private IEnumerator EndSkillAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}