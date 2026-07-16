using Data;
using System.Buffers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static Define;

public class Boss : Creature
{
    #region Data
    public Data.BossData BossData { get; private set; }
    public float TargetDistance { get; set; }
    #endregion
    #region Variables
    public float ReactionSpeed { get; protected set; }
    public ESkillSlot NextSkillSlot { get; set; } = ESkillSlot.None;

    public BossAi _bossAi { get; set; }
    public UI_Boss _bossUi { get; set; }
    #endregion


    #region StateMachine

    public BossStateMachine _stateMachine;
    // Input Response
    //public BossState_ZInput _zInputState { get; protected set; } = new BossState_ZInput();

    // Battle Stances
    public BossState_Chase _chaseState;
    public BossState_Engage _engageState;
    public BossState_StanceChange _changeState;
    public BossState_Stop _stopState;

    // Hit Response
    public BossState_Airborne _airborneState;
    public BossState_Hitstun _hitstunState;
    public BossState_Stagger _staggerState;
    public BossState_Knockdown _knockdownState;
    public BossState_LastHit _lastHitState;
    public BossState_WakeUp _wakeUpState;

    // Movements
    public BossState_Death _deathState;
    public BossState_Fall _fallState;
    public BossState_Idle _idleState;
    public BossState_Land _landState;
    public BossState_Patrol _patrolState;

    // Skills
    public BossState_Skill _skillState;
    public BossState_Skill_Mid _skillMidState;
    public BossState_Skill_Ending _skillEndingState;
    public BossState_Skill_Jump _skillJumpState;
    #endregion
    public override void OnSkillEnd(SkillBase skill)
    {
        base.OnSkillEnd(skill);
        switch (CreatureState)
        {
            case ECreatureState.Idle:
                _stateMachine.ChangeState(IsGrounded ? _skillMidState : _fallState);
                break;

            default:
                _stateMachine.ChangeState(IsGrounded ? _skillMidState : _fallState);
                break;
        }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // Layer
        gameObject.layer = LayerMask.NameToLayer("Boss");
        CreatureType = ECreatureType.Boss;

        ModifyArmor(0);
        ModifyDamage(0);
        IsCreatureShoved = 0;

        // Ai Controller
        _bossAi = new BossAi(this);

        // StateMachine
        _stateMachine = new BossStateMachine(this);
        #region StateMachine Set
        // Battle Stances
        _chaseState = new(this, _stateMachine);
        _engageState = new(this, _stateMachine);
        _changeState = new(this, _stateMachine);
        _stopState = new(this, _stateMachine);

        // Hit Response
        _airborneState = new(this, _stateMachine);
        _hitstunState = new(this, _stateMachine);
        _staggerState = new(this, _stateMachine);
        _knockdownState = new(this, _stateMachine);
        _lastHitState = new(this, _stateMachine);

        // Movements
        _deathState = new(this, _stateMachine);
        _fallState = new(this, _stateMachine);
        _idleState = new(this, _stateMachine);
        _landState = new(this, _stateMachine);
        _patrolState = new(this, _stateMachine);
        _wakeUpState = new(this, _stateMachine);

        // Skills
        _skillState = new(this, _stateMachine);
        _skillMidState = new(this, _stateMachine);
        _skillEndingState = new(this, _stateMachine);
        _skillJumpState = new(this, _stateMachine); // 점프 후 나오는 스킬인데 점프 모션이 따로 있는 경우
        #endregion

        // Boss UI
        _bossUi = Managers.UI.MakeWorldSpaceUI<UI_Boss>();
        Managers.Game._uiBoss = _bossUi;
        _bossUi.SetBoss(this);

        return true;
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        BossData = CreatureData as BossData;

        // CreatureWeapon
        if (BossData.WoodWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Sword;
        else if (BossData.FireWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Dagger;
        else if (BossData.EarthWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Hammer;
        else if (BossData.MetalWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.BattleAxe;
        else if (BossData.WaterWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Shield;
        else
            CreatureWeapon = ECreatureWeapon.None;

        // Boss Stats
        JumpForce = BossData.JumpForce;
        CoyoteTime = BossData.CoyoteTime;
        ReactionSpeed = BossData.ReactionSpeed;

        // State
        CreatureState = ECreatureState.Idle;

        // State Machine
        _stateMachine.ChangeState(_idleState);

        // Controls

        // Temp
        // SuperArmor

    }
    
    public override void Update()
    {
        base.Update();
        _stateMachine?.Update();

        TargetDistance = transform.position.x - Managers.Object.Player.transform.position.x;
    }

    private void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();

        if (CreatureSuperArmor <= 0)
            _bossUi.gameObject.SetActive(false);
        else
            _bossUi.gameObject.SetActive(true);
    }

    #region Battle
    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        base.OnDamaged(attacker, skill);
        IsAggro = true;
    }

    public override void OnSuperArmorBreak(BaseObject attacker, SkillBase skill, float uiPos)
    {
        base.OnSuperArmorBreak(attacker, skill, uiPos);
        
        ESkillType skillType = skill.SkillType;
        BossStateBase _currentState = _stateMachine?.CurrentState;
        if (_currentState is BossState_HitResponse)
        {
            BossState_HitResponse _hitResponse = _currentState as BossState_HitResponse;
            skillType = _hitResponse.GetNextHitResponse(skill);
        }

        switch (skillType)
        {
            case ESkillType.Airborne:
                _stateMachine?.ChangeState(_airborneState);
                return;
            case ESkillType.Hitstun:
                _stateMachine?.ChangeState(_hitstunState);
                return;
            case ESkillType.Stagger:
                _stateMachine?.ChangeState(_staggerState);
                return;
            case ESkillType.Knockdown:
                _stateMachine?.ChangeState(_knockdownState);
                return;
            case ESkillType.LastHit:
                _stateMachine?.ChangeState(_lastHitState);
                return;
            case ESkillType.WakeUp:
                _stateMachine?.ChangeState(_wakeUpState);
                return;
            default:
                _stateMachine?.ChangeState(_hitstunState);
                return;
        }
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
    {
        base.OnDead(attacker, skill);
        Managers.Save.DeadCreatures.Add(SpawnPointID);
        _stateMachine.ChangeState(_deathState);
    }
    #endregion

    #region Input System
    public float _lastSkillTime { get; set; }
    //float _comboResetTime = 0.5f;
    public void InputChange(ESkillSlot slot)
    {
        SkillKey key;
        if (NowKey.Weapon == CreatureWeapon &&
            NowKey.State == CreatureState &&
            NowKey.Slot == slot)
        {
            key = new SkillKey(CreatureWeapon, CreatureState, slot, NowKey.Combo + 1);
        }
        else
        {
            key = new SkillKey(CreatureWeapon, CreatureState, slot, 0);
        }

        SkillExcute(key);
    }

    public void SkillExcute(SkillKey key)
    {
        SkillComponent skills;
        switch (CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                skills = Weapons.WoodWeapon.Skills;
                break;
            case ECreatureWeapon.BattleAxe:
                skills = Weapons.MetalWeapon.Skills;
                break;
            default:
                skills = Weapons.WoodWeapon.Skills;
                break;
        }

        // Null 체크
        if (skills._skillMap.TryGetValue(key, out SkillBase stateSkill) == false)
            return;
        if (stateSkill == null)
            return;

        // 쿨타임 체크
        if (skills.ActiveSkills.Contains(stateSkill) == false)
            return;

        NowSkill = stateSkill;
        NowKey = key;
        _stateMachine.ChangeState(_skillState);
        return;
    }

    public bool IsValidKey(ESkillSlot slot)
    {
        SkillKey key;
        if (NowKey.Weapon == CreatureWeapon &&
            NowKey.State == CreatureState &&
            NowKey.Slot == slot)
        {
            key = new SkillKey(CreatureWeapon, CreatureState, slot, NowKey.Combo + 1);
        }
        else
        {
            key = new SkillKey(CreatureWeapon, CreatureState, slot, 0);
        }

        SkillComponent skills;
        switch (CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                skills = Weapons.WoodWeapon.Skills;
                break;
            case ECreatureWeapon.BattleAxe:
                skills = Weapons.MetalWeapon.Skills;
                break;
            default:
                skills = Weapons.WoodWeapon.Skills;
                break;
        }

        return skills._skillMap.TryGetValue(key, out SkillBase stateSkill);
    }

    private void OnDestroy()
    {
        if (_bossUi)
            Destroy(_bossUi.gameObject);
    }
    #endregion
}
