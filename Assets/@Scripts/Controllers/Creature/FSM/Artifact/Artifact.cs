using Data;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;
using static UnityEngine.GraphicsBuffer;

public class Artifact : Creature
{
    public UI_Artifact _artifactUi { get; set; }
    public EArtifactType ArtifactType { get; private set; }
    public int ArtifactCounter {  get; protected set; }

    #region StateMachine
    public ArtifactStateMachine _stateMachine;

    // HitResponse
    public ArtifactState_HitResponse _hitState;
    public ArtifactState_Break _breakState;
    public ArtifactState_Death _deathState;

    // Movements
    public ArtifactState_Idle _idleState;

    // Skills
    public ArtifactState_Skill _skillState;
    #endregion

    public override void OnSkillEnd(SkillBase skill)
    {
        base.OnSkillEnd(skill);
        _stateMachine.ChangeState(_idleState);
    }
    public Data.ArtifactData ArtifactData { get; private set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = ECreatureType.Artifact;
        ArtifactType = EArtifactType.Earth;

        // StateMachine
        _stateMachine = new ArtifactStateMachine(this);
        #region StateMacine Set
        _hitState = new(this, _stateMachine);
        _breakState = new(this, _stateMachine);
        _deathState = new(this, _stateMachine);

        _idleState = new(this, _stateMachine);

        _skillState = new(this, _stateMachine);
        #endregion

        // Artifact UI
        _artifactUi = Managers.UI.MakeWorldSpaceUI<UI_Artifact>();
        _artifactUi.SetArtifact(this);

        
        return true;
    }

    public override void SetInfo(int templateID)
    {
        base.SetInfo(templateID);

        ArtifactData = CreatureData as ArtifactData;

        // CreatureWeapon
        if (ArtifactData.BarrierId != 0)
        {
            CreatureWeapon = ECreatureWeapon.Barrier;
            ModifyArmor(5);
            ModifyCounter(3);
        }

        // Artifact Skill 설정
        if (CreatureWeapon == ECreatureWeapon.Barrier)
        {
            NowKey = new SkillKey(ECreatureWeapon.Barrier, ECreatureState.Idle, ESkillSlot.A, 0);
            SkillComponent skills = Weapons.Barrier.Skills;

            // Null 체크
            if (skills._skillMap.TryGetValue(NowKey, out SkillBase stateSkill) == false)
                Debug.Log("Artifact Skill Missing");

            NowSkill = stateSkill;
        }

        // State
        CreatureState = ECreatureState.Idle;

        // State Machine
        _stateMachine.ChangeState(_idleState);
        LookLeft = false;

    }

    public override void Update()
    {
        base.Update();
        _stateMachine?.Update();
    }

    public void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();

        if (CreatureSuperArmor <= 0)
            _artifactUi.gameObject.SetActive(false);
        else
            _artifactUi.gameObject.SetActive(true);
    }

    #region Battle
    public event Action<int> OnCounterChanged;
    public void ModifyCounter(int count)
    {
        OnCounterChanged?.Invoke(count);
        ArtifactCounter = count;
    }

    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        base.OnDamaged(attacker, skill);

        // Counter
        ModifyCounter(ArtifactCounter - 1);
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
    {
        base.OnDead(attacker, skill);
        Managers.Save.DeadCreatures.Add(SpawnPointID);
        _stateMachine.ChangeState(_deathState);
    }

    public override void OnSuperArmorBreak(BaseObject attacker, SkillBase skill, float uiPos)
    {
        base.OnSuperArmorBreak(attacker, skill, uiPos);
        Managers.Sound.Play(ESound.Effect, "Stagger", volume: 0.3f);
        _stateMachine?.ChangeState(_breakState);
    }

    private void OnDestroy()
    {
        if (_artifactUi)
            Destroy(_artifactUi.gameObject);
    }
    #endregion
}
