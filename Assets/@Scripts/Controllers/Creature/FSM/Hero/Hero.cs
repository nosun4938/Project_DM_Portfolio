using Data;
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static Define;

public class Hero : Creature
{
    #region Data
    public Data.HeroData HeroData { get; private set; }
    #endregion
    #region Variables
    public Dictionary<ESkillSlot, bool> _inputPressed = new Dictionary<ESkillSlot, bool>();
    public Dictionary<ESkillSlot, float> _lastInputTime = new Dictionary<ESkillSlot, float>();

    public bool HasJumped { get; set; } = false;
    public bool IsJumpPressed { get; set; } = false;
    public float CoyoteTimeCounter { get; set; }
    public float JumpBufferTimeCounter { get; set; }
    public UI_Hero HeroUi { get; set; }

    public Transform InteractionCheck { get; set; }
    #endregion

    #region StateMachine
    public HeroStateMachine _stateMachine;

    // Skills
    public HeroState_Skill _skillState;
    public HeroState_IdleSkill_Mid _skillMidState;
    public HeroState_IdleSkill_Ending _skillEndingState;

    // Hit Response
    public HeroState_Airborne _airborneState;
    public HeroState_Hitstun _hitstunState;
    public HeroState_Stagger _staggerState;
    public HeroState_Knockdown _knockdownState;
    public HeroState_LastHit _lastHitState;
    public HeroState_WakeUp _wakeUpState;

    // Movements
    public HeroState_Death _deathState;
    public HeroState_Fall _fallState;
    public HeroState_Idle _idleState;
    public HeroState_Jump _jumpState;
    public HeroState_Land _landState;
    public HeroState_Run _runState;
    public HeroState_Stop _stopState;

    public override void OnSkillEnd(SkillBase skill)
    {
        base.OnSkillEnd(skill);
        //Debug.Log($"On Skill End : {skill}");
        if (skill is Sword_Shift || skill is Sword_C_Airborne)
            _stateMachine.ChangeState(IsGrounded ? _skillEndingState : _fallState);
        else
            _stateMachine.ChangeState(IsGrounded ? _skillMidState : _fallState);
    }
    #endregion
    public override bool Init()
    {
        //Debug.LogWarning($"Hero.Init() 호출됨\n{Environment.StackTrace}");
        if (base.Init() == false)
            return false;

        // Input System
        foreach (ESkillSlot slot in Enum.GetValues(typeof(ESkillSlot)))
        {
            _inputPressed[slot] = false;
        }

        // Layer
        gameObject.layer = LayerMask.NameToLayer("Player");
        CreatureType = ECreatureType.Hero;

        ModifyArmor(0);
        ModifyDamage(0);
        IsCreatureShoved = 0;

        // StateMachine
        _stateMachine = new HeroStateMachine(this);
        #region StateMachine Set
        // Skills
        _skillState = new(this, _stateMachine);
        _skillMidState = new(this, _stateMachine);
        _skillEndingState = new(this, _stateMachine);

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
        _jumpState = new(this, _stateMachine);
        _landState = new(this, _stateMachine);
        _runState = new(this, _stateMachine);
        _stopState = new(this, _stateMachine);
        _wakeUpState = new(this, _stateMachine);
        #endregion

        // Hero UI
        HeroUi = Managers.UI.MakeWorldSpaceUI<UI_Hero>();
        Managers.Game._uiHero = HeroUi;
        HeroUi.SetHero(this);

        return true;
    }

    public override void Update()
    {
        base.Update();
        HandleBufferedInput();
        HandleHoldInput();

        _stateMachine?.Update();
    }

    public void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
        
        if (CreatureSuperArmorBreak <= 0)
            HeroUi.gameObject.SetActive(false);
        else
            HeroUi.gameObject.SetActive(true);
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);
        HeroData = CreatureData as HeroData;

        // CreatureWeapon
        if (HeroData.WoodWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Sword;
        else if (HeroData.FireWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Dagger;
        else if (HeroData.EarthWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Hammer;
        else if (HeroData.MetalWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.BattleAxe;
        else if (HeroData.WaterWeaponId != 0)
            CreatureWeapon = ECreatureWeapon.Shield;
        else
            CreatureWeapon = ECreatureWeapon.None;

        // Stats
        JumpForce = HeroData.JumpForce;
        CoyoteTime = HeroData.CoyoteTime;

        // State
        CreatureState = ECreatureState.Idle;

        // State Machine
        _stateMachine.ChangeState(_idleState);
    }


    #region Input System
    public float _lastSkillTime { get; set; }
    float _repeatDelay = 0.3f;

    // 플레이어 인풋에 반응이 필요한 경우
    public void NotifyInput(ESkillSlot slot)
    {
        Managers.Game.OnHeroInput(slot);
    }

    public struct BufferedInput
    {
        public ESkillSlot Slot;
        public float Time;
    }

    private List<BufferedInput> _inputBuffer = new List<BufferedInput>();
    private float _inputBufferTime = 0.2f;

    public void BufferInput(ESkillSlot slot)
    {
        _inputBuffer.Add(new BufferedInput
        {
            Slot = slot,
            Time = Time.time
        });
    }

    public bool TryConsumeBufferInput(Func<ESkillSlot, bool> canUse, out ESkillSlot slot)
    {
        slot = ESkillSlot.None;

        for (int i = 0; i < _inputBuffer.Count; i++)
        {
            var input = _inputBuffer[i];

            if (Time.time - input.Time > _inputBufferTime)
            {
                _inputBuffer.RemoveAt(i);
                i--;
                continue;
            }

            if (canUse(input.Slot) == false)
                continue;

            slot = input.Slot;
            _inputBuffer.RemoveAt(i);
            return true;
        }

        return false;
    }

    public void HandleHoldInput()
    {
        foreach (var pair in _inputPressed)
        {
            ESkillSlot slot = pair.Key;
            bool isPressed = pair.Value;

            if (!isPressed)
                continue;

            //if (!Owner._lastInputTime.TryGetValue(slot, out float lastTime))
            //continue;

            if (Time.time - _lastInputTime[slot] < _repeatDelay)
                continue;
            _lastInputTime[slot] = Time.time;

            BufferInput(slot);
        }
    }

    public void HandleBufferedInput()
    {
        if (TryConsumeBufferInput(CanUseSkill, out ESkillSlot slot) == false)
            return;

        // 점프
        if (slot == ESkillSlot.C)
        {
            if (CanJump())
            {
                _stateMachine.ChangeState(_jumpState);
                return;
            }
            
            if (CreatureState == ECreatureState.Airborne)
            {
                SkillExcute(new SkillKey(CreatureWeapon, CreatureState, slot, 0));
                return;
            }   
        }

        if (slot == ESkillSlot.F)
        {
            //InteractionDetector.TryInteract();
        }

        // Skill 실행
        if (SkillEnd)
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

    }

    public bool CanJump()
    {
        return CoyoteTimeCounter > 0f &&
            HasJumped == false &&
            SkillEnd == true &&
            CreatureState != ECreatureState.Airborne;
    }
    public bool CanUseSkill(ESkillSlot slot)
    {
        // C 입력이 들어왔을 때
        if (slot == ESkillSlot.C)
        {
            // 점프
            if (CreatureState != ECreatureState.Airborne)
                return CanJump();
            // 공중 회피
            else
                return true;
        }

        // F 입력이 들어왔을 때
        if (slot == ESkillSlot.F)
        {
            return true;
        }

        // 회피기 사용 중 Z
        if (NowKey.Slot == ESkillSlot.Shift && slot == ESkillSlot.Z)
        {
            CreatureState = ECreatureState.Dash;
            _stateMachine.ChangeState(_skillMidState);
            return SkillEnd;
        }

        // 공중에서 사용가능한 스킬 존재
        if ((slot == ESkillSlot.Z || slot == ESkillSlot.X) && CoyoteTimeCounter < 0)
            return SkillEnd;

        // 그 외 스킬
        if (CoyoteTimeCounter < 0)
            return false;
        else
            return SkillEnd;
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

        //Debug.Log(key.ToString());

        // Null 체크
        if (skills._skillMap.TryGetValue(key, out SkillBase stateSkill) == false)
            return;
        if (stateSkill == null)
            return;

        // 쿨타임 체크
        if (skills.ActiveSkills.Contains(stateSkill) == false)
        {
            Debug.Log($"{stateSkill} is on CoolDown");
            return;
        }

        NowSkill = stateSkill;
        NowKey = key;

        _stateMachine.ChangeState(_skillState);
        return;
    }

    public void OnMove(InputAction.CallbackContext context) => _stateMachine?.OnMove(context);
    public void OnJump(InputAction.CallbackContext context) => _stateMachine?.OnJump(context);
    public void OnDash(InputAction.CallbackContext context) => _stateMachine?.OnDash(context);

    // Skill
    public void OnZSkill(InputAction.CallbackContext context) => _stateMachine?.OnZSkill(context);
    public void OnXSkill(InputAction.CallbackContext context) => _stateMachine?.OnXSkill(context);
    public void OnASkill(InputAction.CallbackContext context) => _stateMachine?.OnASkill(context);
    public void OnSSkill(InputAction.CallbackContext context) => _stateMachine?.OnSSkill(context);
    public void OnDSkill(InputAction.CallbackContext context) => _stateMachine?.OnDSkill(context);
    public void OnFSkill(InputAction.CallbackContext context) => _stateMachine?.OnFSkill(context);
    #endregion


    #region Battle
    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        base.OnDamaged(attacker, skill);
    }

    public override void OnSuperArmorBreak(BaseObject attacker, SkillBase skill, float uiPos)
    {
        base.OnSuperArmorBreak(attacker, skill, uiPos);

        if (CreatureState == ECreatureState.Jump)
        {
            _stateMachine?.ChangeState(_airborneState);
            return;
        }

        ESkillType skillType = skill.SkillType;
        HeroStateBase _currentState = _stateMachine?.CurrentState;
        if (_currentState is HeroState_HitResponse)
        {
            HeroState_HitResponse _hitResponse = _currentState as HeroState_HitResponse;
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
        Managers.Save.DeadCreatures.Clear();
        _stateMachine.ChangeState(_deathState);
    }

    public void Respawn()
    {
        Managers.Map.RespawnMap();
        transform.position = Managers.Game.NowMap.Tilemap.GetCellCenterWorld(Managers.Game.Player.RespawnSpot);

        ModifyHp(MaxHp);
        CreatureState = ECreatureState.Idle;
        HitCircle.gameObject.SetActive(true);
        _stateMachine.ChangeState(_idleState);
    }
    #endregion
}
