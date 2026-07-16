using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static Define;

public class Monster : Creature
{
    #region Data
    public Data.MonsterData MonsterData { get; private set; }
    #endregion

    #region Variables
    public float TargetDistance { get; set; }
    public float TargetDistanceY { get; set; }
    public ESkillSlot NextSkillSlot { get; set; } = ESkillSlot.None;
    public UI_Monster _monsterUi { get; set; }
    #endregion


    #region StateMachine
    public MonsterStateMachine _stateMachine;

    // Battle State
    public MonsterState_Chase _chaseState;
    public MonsterState_Engage _engageState;

    // Hit Response
    public MonsterState_Airborne _airborneState;
    public MonsterState_Hitstun _hitstunState;
    public MonsterState_Stagger _staggerState;
    public MonsterState_Knockdown _knockdownState;
    public MonsterState_LastHit _lastHitState;
    public MonsterState_WakeUp _wakeUpState;

    // Movements
    public MonsterState_Death _deathState;
    public MonsterState_Fall _fallState;
    public MonsterState_Idle _idleState;
    public MonsterState_Land _landState;
    public MonsterState_Patrol _patrolState;
    public MonsterState_Stop _stopState;

    // Skills
    public MonsterState_Skill _skillState;
    public MonsterState_Skill_Mid _skillMidState;
    public MonsterState_Skill_Ending _skillEndingState;

    public override void OnSkillEnd(SkillBase skill)
    {
        base.OnSkillEnd(skill);

        switch (skill)
        {
            default:
                _stateMachine.ChangeState(_skillMidState);
                break;
        }
    }
    #endregion
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        // Layer
        gameObject.layer = LayerMask.NameToLayer("Monster");
        CreatureType = ECreatureType.Monster;

        ModifyArmor(0);
        ModifyDamage(0);
        IsCreatureShoved = 0;

        // StateMachine
        _stateMachine = new MonsterStateMachine(this);
        #region State Machine Set
        // Battle Stance
        _chaseState = new(this, _stateMachine);
        _engageState = new(this, _stateMachine);

        // HitResponses
        _airborneState = new(this, _stateMachine);
        _hitstunState = new(this, _stateMachine);
        _staggerState = new(this, _stateMachine);
        _knockdownState = new(this, _stateMachine);
        _lastHitState = new(this, _stateMachine);
        _wakeUpState = new(this, _stateMachine);

        // Movements
        _deathState = new(this, _stateMachine);
        _fallState = new(this, _stateMachine);
        _idleState = new(this, _stateMachine);
        _landState = new(this, _stateMachine);
        _patrolState = new(this, _stateMachine);
        _stopState = new(this, _stateMachine);

        // Skills
        _skillState = new(this, _stateMachine);
        _skillMidState = new(this, _stateMachine);
        _skillEndingState = new(this, _stateMachine);
        #endregion

        // Monster UI
        _monsterUi = Managers.UI.MakeWorldSpaceUI<UI_Monster>();
        Managers.Game._uiMonster = _monsterUi;
        _monsterUi.SetMonster(this);

        return true;
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);

        MonsterData = CreatureData as MonsterData;

        // CreatureWeapon
        if (MonsterData.WoodWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Sword;
        else if (MonsterData.FireWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Dagger;
        else if (MonsterData.EarthWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Hammer;
        else if (MonsterData.MetalWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.BattleAxe;
        else if (MonsterData.WaterWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Shield;
        else
            CreatureWeapon = ECreatureWeapon.None;

        // Stats
        JumpForce = MonsterData.JumpForce;
        CoyoteTime = MonsterData.CoyoteTime;

        // State
        CreatureState = ECreatureState.Idle;

        // State Machine
        _stateMachine.ChangeState(_idleState);
    }

    public override void Update()
    {
        base.Update();
        _stateMachine?.Update();

        TargetDistance = transform.position.x - Managers.Object.Player.transform.position.x;
        TargetDistanceY = transform.position.y - Managers.Object.Player.transform.position.y;
    }
    public void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
        if (CreatureSuperArmor <= 0)
            _monsterUi.gameObject.SetActive(false);
        else
            _monsterUi.gameObject.SetActive(true);
    }

    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        base.OnDamaged(attacker, skill);
        IsAggro = true;
    }
    public override void OnSuperArmorBreak(BaseObject attacker, SkillBase skill, float uiPos)
    {
        base.OnSuperArmorBreak(attacker, skill, uiPos);

        ESkillType skillType = skill.SkillType;
        MonsterStateBase _currentState = _stateMachine?.CurrentState;
        if (_currentState is MonsterState_HitResponse)
        {
            MonsterState_HitResponse _hitResponse = _currentState as MonsterState_HitResponse;
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
            case ECreatureWeapon.Dagger:
                skills = Weapons.FireWeapon.Skills;
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
            case ECreatureWeapon.Dagger:
                skills = Weapons.FireWeapon.Skills;
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
        if (_monsterUi)
            Destroy(_monsterUi.gameObject);
    }
    #endregion
}
