using UnityEngine;
using static Define;

public class BossState_Engage : BossStateBase
{
    public BossState_Engage(Boss owner, BossStateMachine stateMachine) : base(owner, stateMachine) { }
    
    float _waitTime = 1f;
    float _timer = 0f;
    ESkillSlot _skillSlot;

    public override void Enter()
    {
        base.Enter();
        FaceToTarget();
        Owner.CreatureState = ECreatureState.Idle;
        Owner.Horizontal = 0f;
        Owner.MoveSpeed = 40f;

        Owner.ModifyDamage(0);
        Owner.ModifyArmor(0);

        _waitTime = Random.Range(0.2f, 0.5f);
        Owner.Animator.Play("Metal_Engage", 0, 0f);
    }

    public override void Update()
    {
        base.Update();

        if (Owner.IsGrounded == false)
        {
            _stateMachine.ChangeState(Owner._fallState);
            return;
        }
        
        _timer += Time.deltaTime;
        if (_timer >= _waitTime)
        {
            _skillSlot = Owner._bossAi.DistanceSlot();

            // Chase
            if (_skillSlot == ESkillSlot.None)
            {
                _stateMachine.ChangeState(Owner._chaseState);
                return;
            }

            // AirCrash
            if (_skillSlot == ESkillSlot.F)
            {
                _skillSlot = ESkillSlot.X;
                Owner.NextSkillSlot = _skillSlot;
                _stateMachine.ChangeState(Owner._skillJumpState);
                return;
            }

            // 그 외 스킬
            {
                Owner.NextSkillSlot = _skillSlot;
                NextSkillSlotPress();
                return;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _timer = 0f;
    }
}