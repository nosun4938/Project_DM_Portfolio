using Data;
using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using static Define;

public class Creature : BaseObject
{
    #region Components & Enums
    public BoxCollider2D Collider { get; private set; }
    public WeaponComponent Weapons { get; protected set; }
    public Data.CreatureData CreatureData { get; private set; }
    
    public BoxCollider2D HitCircle {  get; private set; }
    public Transform GroundCheck { get; protected set; }
    public Transform WallCheck { get; protected set; }
    public Transform HitCheck { get; private set; }
    public Vector3 InitVector { get; set; }
    public String SpawnPointID { get; set; }

    public ECreatureType CreatureType { get; protected set; } = ECreatureType.None;
    public ECreatureWeapon CreatureWeapon { get; protected set; } = ECreatureWeapon.None;

    protected ECreatureState _creatureState = ECreatureState.None;
    public virtual ECreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            if (_creatureState != value)
            {
                _creatureState = value;
            }
        }
    }
    public float Horizontal { get; set; }
    public bool SkillHit { get; set; } = false;
    public bool SkillEnd { get; set; } = true;
    public float SkillEndLength { get; set; }
    public SkillBase NowSkill { get; set; }
    public SkillKey NowKey { get; set; }

    public bool IsAggro { get; set; } = false;
    public bool IsGrounded { get; set; } = false;
    public Vector2 LastPosition { get; set; }
    public int IsCreatureShoved { get; set; }
    public float CreatureSuperArmor { get; set; }
    public float CreatureSuperArmorBreak { get; set; }

    public bool CheckIsGrounded()
    {
        //return Physics2D.OverlapCircle(WallCheck.position, 0.55f, LayerMask.GetMask("Ground"));
        return Physics2D.OverlapBox(GroundCheck.position, new Vector2(CreatureData.HitBox.Size.x - 0.1f, 0.5f), 0f, LayerMask.GetMask("Ground"));
    }

    public bool CheckIsBounded()
    {
        return Physics2D.OverlapCircle(WallCheck.position, 0.55f, LayerMask.GetMask("Wall"));
    }

    private void OnDrawGizmos()
    {
    #if UNITY_EDITOR
        if (GroundCheck == null)
            return;

        Handles.color = new Color(1, 0, 0, 0.4f);
        Handles.DrawWireCube(GroundCheck.position, new Vector2(CreatureData.HitBox.Size.x, 0.5f));
    #endif
    }


    public SkillComponent Skills => Weapons.GetCurrentWeapon()?.Skills;
    #endregion

    #region Stats
    // Creature
    public float Hp { get; protected set; }
    public float MaxHp {  get; protected set; }
    public float MoveSpeed { get; set; }
    public float Money { get; set; }

    // Boss, Hero
    public float JumpForce { get; set; }
    public float CoyoteTime { get; set; }
    #endregion

    #region UI Events
    public event Action<float, float> OnHpChanged;
    public event Action<float> OnArmorChanged;
    public event Action<float> OnDamageChanged;

    private void OnDisable()
    {

    }

    public virtual void ModifyArmor(float armor)
    {
        OnArmorChanged?.Invoke(armor);
        CreatureSuperArmor = armor;
    }
    public virtual void ModifyDamage(float damage)
    {
        OnDamageChanged?.Invoke(damage);
        CreatureSuperArmorBreak = damage;
    }

    public virtual void ModifyHp(float hp)
    {
        OnHpChanged?.Invoke(hp, MaxHp);
        Hp = hp;
    }
    #endregion
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        ObjectType = EObjectType.Creature;

        // Physics Checker
        GroundCheck = transform.Find("@groundCheck");
        if (GroundCheck == null)
        {
            GameObject obj = new GameObject("@groundCheck");
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            GroundCheck = obj.transform;
        }

        WallCheck = transform.Find("@wallCheck");
        if (WallCheck == null)
        {
            GameObject obj = new GameObject("@wallCheck");
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            WallCheck = obj.transform;
        }

        // Collider 이름을 데이터 상 HitBox로 만들어버려서, 피격 판정 이름을 HitCircle로 지음
        // 그래서 이름은 Circle인데 형태는 Box임 (개발 초기에는 형태도 Circle이었음)
        HitCheck = transform.Find("@hitCircle");
        if (HitCheck == null)
        {
            GameObject obj = new GameObject("@hitCheck");
            obj.transform.parent = transform;
            obj.transform.localPosition = new Vector3(0, 0.25f, 0);
            obj.layer = LayerMask.NameToLayer("HitCircle");
            HitCheck = obj.transform;
        }

        // Events
        OnHpChanged?.Invoke(Hp, MaxHp);

        return true;
    }

    public virtual void SetInfo(int templateID)
    {
        DataTemplateID = templateID;

        if (CreatureType == ECreatureType.Hero)
            CreatureData = Managers.Data.HeroDic[templateID];
        else if (CreatureType == ECreatureType.Monster)
            CreatureData = Managers.Data.MonsterDic[templateID];
        else if (CreatureType == ECreatureType.Boss)
            CreatureData = Managers.Data.BossDic[templateID];
        else if (CreatureType == ECreatureType.Artifact)
            CreatureData = Managers.Data.ArtifactDic[templateID];

        gameObject.name = $"{CreatureData.DataID}_{CreatureData.DescriptionTextID}";

        // Collider
        Collider = gameObject.GetOrAddComponent<BoxCollider2D>();
        Collider.offset = CreatureData.HitBox.Offset;
        Collider.size = CreatureData.HitBox.Size;

        HitCircle = HitCheck.gameObject.GetOrAddComponent<BoxCollider2D>();
        HitCircle.offset = CreatureData.HitCircle.Offset;
        HitCircle.size = CreatureData.HitCircle.Size;
        HitCircle.isTrigger = true;
        
        // RigidBody
        Rigidbody.mass = CreatureData.Mass;
        Rigidbody.gravityScale = 5.0f;

        // Animator
        Animator animator = GetComponent<Animator>();
        if (animator == null)
            animator = gameObject.GetOrAddComponent<Animator>();

        animator.runtimeAnimatorController = Managers.Resource.Load<RuntimeAnimatorController>(CreatureData.AnimatorName);
        
        // Sprite Renderer
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.GetOrAddComponent<SpriteRenderer>();

        Creature creature = gameObject.GetComponent<Creature>();
        switch (creature.CreatureType)
        {
            case ECreatureType.Hero:
                spriteRenderer.sortingOrder = SortingLayers.HERO;
                break;
            case ECreatureType.Monster:
                spriteRenderer.sortingOrder = SortingLayers.MONSTER;
                break;
            case ECreatureType.Boss:
                spriteRenderer.sortingOrder = SortingLayers.BOSS;
                break;
            case ECreatureType.Artifact:
                spriteRenderer.sortingOrder= SortingLayers.ARTIFACT;
                break;
        }

        // Stat
        MaxHp = CreatureData.MaxHp;
        Hp = CreatureData.MaxHp;
        MoveSpeed = CreatureData.MoveSpeed;
        Money = CreatureData.Money;

        // State
        CreatureState = ECreatureState.Idle;

        // Weapon
        Weapons = gameObject.GetOrAddComponent<WeaponComponent>();
        Weapons.SetInfo(this, CreatureData);
    }
    public virtual void Update()
    {
        IsGrounded = CheckIsGrounded();

        if (IsGrounded)
            LastPosition = transform.position;
    }

    #region Battle
    public override void OnDamaged(BaseObject attacker, SkillBase skill)
    {
        base.OnDamaged(attacker, skill);
        if (attacker.IsValid() == false)
            return;

        Creature creature = attacker as Creature;
        if (creature == null)
            return;

        // 기본 타격 이펙트 출력
        switch (skill.HitType)
        {
            case ESkillEffectType.Hit:
                float dice = UnityEngine.Random.Range(-2f, 2f);
                float posX = transform.position.x + dice;
                float posY = transform.position.y + dice;
                Managers.Combat.PlayHit(new Vector2(posX, posY + 8f), LookLeft);
                break;
            case ESkillEffectType.Slash:
                float angle = UnityEngine.Random.Range(-30f, 30f);
                Managers.Combat.PlaySlash(new Vector2(transform.position.x, transform.position.y + 8f), angle, LookLeft);
                break;
        }

        // 슈퍼아머 계산
        float oriSuperArmor = CreatureSuperArmor;
        CreatureSuperArmor -= skill.SuperArmorBreak;
        ModifyArmor(CreatureSuperArmor);

        bool _isBroken = false;
        if (CreatureSuperArmor < 0)
        {
            _isBroken = true;
        }

        // TEMP 슈퍼아머 UI위치
        float uiPos;
        if (this is Artifact)
            uiPos = 28f;
        else
            uiPos = 20f;

        // 슈퍼아머 파괴시
        if (_isBroken)
        {
            if (oriSuperArmor > 0)
                Managers.Combat.PlayArmorCrash(new Vector2(transform.position.x, transform.position.y + uiPos), LookLeft);

            OnSuperArmorBreak(attacker, skill, uiPos);
            IsCreatureShoved = skill.ShoveAttck;
        }
        else
        {
            OnCombo(skill, uiPos);
        }

        if (Hp <= 0)
        {
            OnDead(attacker, skill);
            HitCircle.gameObject.SetActive(false);
            return;
        }
    }

    public virtual void OnSuperArmorBreak(BaseObject attacker, SkillBase skill, float uiPos)
    {
        // 피격 방향 설정
        {
            float attackDirection = gameObject.transform.position.x - attacker.transform.position.x;

            if (attackDirection < 0)
                LookLeft = true;
            else if (attackDirection > 0)
                LookLeft = false;
        }

        // HP 계산
        {
            float finalDamage = skill.Damage;
            Hp -= finalDamage;
            Hp = Mathf.Clamp(Hp, 0, MaxHp);

            OnHpChanged?.Invoke(Hp, MaxHp);
            Debug.Log($"Hp: {Hp}");
        }

        // 피격 이펙트 출력
        switch (skill.HitType)
        {
            case ESkillEffectType.Hit:
                Managers.Combat.PlayCrash(new Vector2(transform.position.x, transform.position.y + 8f), LookLeft);
                break;
            case ESkillEffectType.Slash:
                float angle = UnityEngine.Random.Range(-30f, 30f);
                Managers.Combat.PlaySlashBreak(new Vector2(transform.position.x, transform.position.y + 8f), angle, LookLeft);
                break;
        }
    }
    public virtual void OnCombo(SkillBase skill, float uiPos)
    {
        Managers.Combat.PlayArmorBreak(new Vector2(transform.position.x, transform.position.y + uiPos), LookLeft);
        Managers.Sound.Play(ESound.Effect, "SuperArmor", volume: 0.3f);
    }

    public override void OnDead(BaseObject attacker, SkillBase skill)
    {
        base.OnDead(attacker, skill);
    }
    #endregion


    #region Skills
    public Coroutine _skillCoroutine { get; protected set; }
    public Coroutine _resetKeyCoroutine { get; protected set; }

    // 스킬 코루틴은 단일화해서 관리
    public void StartCreatureCoroutine(IEnumerator coroutine)
    {
        StopCreatureCoroutine();

        _skillCoroutine = StartCoroutine(coroutine);
    }

    public void StopCreatureCoroutine()
    {
        if (_skillCoroutine != null)
        {
            StopCoroutine(_skillCoroutine);
            _skillCoroutine = null;
        }
    }

    public void ResetSkillKey()
    {
        StopResetKeyCoroutine();
        _resetKeyCoroutine = StartCoroutine(CoResetSkillKey(0.3f));
    }
    public void StopResetKeyCoroutine()
    {
        if (_resetKeyCoroutine != null)
        {
            StopCoroutine(_resetKeyCoroutine);
            _resetKeyCoroutine = null;
        }
    }

    IEnumerator CoResetSkillKey(float delay)
    {
        yield return new WaitForSeconds(delay);
        NowKey = default;
    }

    public int GetDirection()
    {
        if (Horizontal != 0)
            return (int)Mathf.Sign(Horizontal);

        return LookLeft ? 1 : -1;
    }
    #endregion
}
