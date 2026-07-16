using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class SkillBase : InitBase
{
    public Creature Owner { get; protected set; }
    public Data.SkillData SkillData { get; private set; }

    public float RemainCoolTime { get; protected set; }
    public int DashDir { get; protected set; }
    protected float Frame = 0.0833f;
    protected float Delay;

    // Data
    public AnimationClip AnimClip { get; protected set; }
    public String SoundKey { get; protected set; }
    public ESkillType SkillType { get; protected set; }
    public float Damage {  get; protected set; }
    public ESkillEffectType HitType {  get; protected set; }
    public float SuperArmor { get; protected set; }
    public float SuperArmorBreak { get; protected set; }
    public int ShoveAttck { get; protected set; }

    public int StartFrame { get; protected set; }
    public int EndFrame { get; protected set; }
    public float MoveRange { get; protected set; }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(Creature owner, int skillTemplateID)
    {
        Owner = owner;
        SkillData = Managers.Data.SkillDic[skillTemplateID];

        // Animation Clip
        AnimClip = GetAnimationClipByName(SkillData.AnimName);

        // Skill Sound
        SoundKey = SkillData.SkillSound;

        // Skill Type
        SkillType = (ESkillType)Enum.Parse(typeof(ESkillType), SkillData.AttackType, true);

        // Damage
        Damage = SkillData.Damage;

        // Skill Effect Type
        HitType = (ESkillEffectType)Enum.Parse(typeof(ESkillEffectType), SkillData.HitType, true);

        // SuperArmor
        SuperArmor = SkillData.SuperArmorLevel;

        // SuperArmorBreak
        SuperArmorBreak = SkillData.SuperArmorBreakLevel;

        // ShoveAttck
        ShoveAttck = SkillData.ShoveAttck;

        // Delay
        Delay = AnimClip != null ? AnimClip.length : 0.1f;

        // Frame
        StartFrame = SkillData.StartFrame;
        EndFrame = SkillData.EndFrame;
        MoveRange = SkillData.MoveRange;
    }

    private void OnDisable()
    {
        if (Managers.Game == null)
            return;
        if (Owner.IsValid() == false)
            return;
    }

    public virtual void DoSkill()
    {
        Owner.ModifyArmor(SuperArmor);
        Owner.ModifyDamage(SuperArmorBreak);

        Owner.Animator.Play(SkillData.AnimName, 0, 0f);

        if (Owner.Skills != null)
        {
            Owner.Skills.ActiveSkills.Remove(this);
            //Debug.Log($"{this} Removed");
        }
        ResetHitTargets();
        StartCoroutine(CoCountdownCooldown());
    }

    protected AnimationClip GetAnimationClipByName(string clipName)
    {
        Animator animator = Owner.Animator;

        if (animator != null)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name == clipName)
                {
                    return clip;
                }
            }
        }

        return null;
    }

    // Animation Event 기반 스킬 피격 판정
    bool performingHit;
    protected virtual void EnableHit()
    {
        if (Owner.NowSkill != this)
            return;

        performingHit = true;
        Managers.Sound.Play(ESound.Effect, SoundKey, volume: 1.0f);
    }

    private void Update()
    {
        if (Owner.NowSkill != this)
            return;

        if (performingHit)
        {
            if (PerformHitDetection())
                Owner.SkillHit = true;
        }     
    }

    protected virtual void DisableHit()
    {
        ResetHitTargets();
        performingHit = false;
    }


    #region Skill Direction
    protected int GetDashDirection()
    {
        //if (Owner.Horizontal != 0)
            //return (int)Mathf.Sign(Owner.Horizontal);

        return Owner.LookLeft ? 1 : -1;
    }
    #endregion

    #region Skill Coroutine
    // 일반적인 스킬 실행, skillMoveType: X축 이동 타입 설정
    protected IEnumerator SkillExcuting(ESkillMoveType skillMoveType)
    {
        yield return new WaitForSeconds(StartFrame * Frame);

        DashDir = GetDashDirection();
        Vector2 startPos = Owner.transform.position;
        Vector2 endPos = startPos + new Vector2(DashDir * MoveRange, 0);

        float elapsed = 0f;
        float totalFrame = (EndFrame - StartFrame) * Frame;

        while (elapsed < totalFrame)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / totalFrame;

            float easedT;
            switch (skillMoveType)
            {
                case (ESkillMoveType.Dash):
                    easedT = 1 - Mathf.Pow(1 - t, 3);
                    break;
                case (ESkillMoveType.Step):
                    easedT = t * t * (3f - 2f * t);
                    break;
                case (ESkillMoveType.Slide):
                    easedT = 1 - Mathf.Pow(1 - t, 2);
                    break;
                case (ESkillMoveType.Heavy):
                    easedT = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                default:
                    easedT = 0;
                    break;
            }

            Vector2 newPos = Vector2.Lerp(startPos, endPos, easedT);
            Owner.Rigidbody.MovePosition(newPos);
            yield return null;
        }

        Owner.Rigidbody.MovePosition(endPos);

        yield return new WaitForSeconds(Delay - EndFrame * Frame);

        string endAnimation = $"{SkillData.AnimName.Replace("_Astart", "_Zend")}";
        AnimClip = GetAnimationClipByName(endAnimation);
        Owner.SkillEndLength = AnimClip != null ? AnimClip.length - 0.2f : 0f;
        Owner.Animator.Play(endAnimation, 0, 0f);
        Owner.OnSkillEnd(this);
    }

    // 공중에서 내려찍는 스킬
    protected IEnumerator AirSkillExcuting()
    {
        Owner.Rigidbody.linearVelocityY = 0f;
        Owner.Rigidbody.gravityScale = 0f;

        yield return new WaitForSeconds(StartFrame * Frame);

        Vector2 startPos = Owner.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(startPos, Vector2.down, 100f, LayerMask.GetMask("Ground"));
        Vector2 endPos;

        if (hit.collider != null)
            endPos = hit.point + Vector2.up * 0.1f;
        else
            endPos = startPos + Vector2.down * MoveRange;

        float elapsed = 0f;
        float totalFrame = (EndFrame - StartFrame) * Frame;

        while (elapsed < totalFrame)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / totalFrame;

            float easedT = t * t;
            Vector2 newPos = Vector2.Lerp(startPos, endPos, easedT);
            Owner.Rigidbody.MovePosition(newPos);
            yield return null;
        }

        Owner.Rigidbody.MovePosition(endPos);
        Owner.CreatureState = ECreatureState.Idle;

        yield return new WaitForSeconds(Delay - EndFrame * Frame);

        string endAnimation = $"{SkillData.AnimName.Replace("_Astart", "_Zend")}";
        AnimClip = GetAnimationClipByName(endAnimation);
        Owner.SkillEndLength = AnimClip != null ? AnimClip.length - 0.2f : 0f;
        Owner.Animator.Play(endAnimation, 0, 0f);
        Owner.OnSkillEnd(this);
    }

    protected IEnumerator BossAirSkillExcuting(float spot)
    {
        Owner.Rigidbody.linearVelocityY = 0f;
        Owner.Rigidbody.gravityScale = 0f;

        yield return new WaitForSeconds(StartFrame * Frame);

        Vector2 startPos = Owner.Rigidbody.position;
        Vector2 targetPos = Managers.Object.Player.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.down, 100f, LayerMask.GetMask("Ground"));
        Vector2 endPos;

        if (hit.collider != null)
            endPos = hit.point + Vector2.up * 0.1f;
        else
            endPos = startPos + Vector2.down * MoveRange;

        if (Owner.LookLeft)
            endPos -= new Vector2(spot, 0);
        else
            endPos += new Vector2(spot, 0);

        float elapsed = 0f;
        float totalFrame = (EndFrame - StartFrame) * Frame;

        while (elapsed < totalFrame)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / totalFrame;

            float easedT = t * t;
            Vector2 newPos = Vector2.Lerp(startPos, endPos, easedT);
            Owner.Rigidbody.MovePosition(newPos);
            yield return null;
        }

        Owner.Rigidbody.MovePosition(endPos);
        Owner.CreatureState = ECreatureState.Idle;

        yield return new WaitForSeconds(Delay - EndFrame * Frame);

        string endAnimation = $"{SkillData.AnimName.Replace("_Astart", "_Zend")}";
        AnimClip = GetAnimationClipByName(endAnimation);
        Owner.SkillEndLength = AnimClip != null ? AnimClip.length - 0.2f : 0f;
        Owner.Animator.Play(endAnimation, 0, 0f);
        Owner.OnSkillEnd(this);
    }


    // 쿨타임 계산
    private IEnumerator CoCountdownCooldown()
    {
        RemainCoolTime = SkillData.CoolTime;

        while (RemainCoolTime > 0)
        {
            RemainCoolTime -= Time.deltaTime;
            yield return null;
        }
        RemainCoolTime = 0;

        if (Owner.Skills != null)
        {
            Owner.Skills.ActiveSkills.Add(this);
            //Debug.Log($"{this} Added");
        }
            
    }
    #endregion

    #region OnHit
    private HashSet<Creature> _alreadyHit = new HashSet<Creature> ();
    protected virtual bool PerformHitDetection()
    {
        Vector2 center = (Vector2)Owner.transform.position + new Vector2(
            Owner.LookLeft ? SkillData.HitBox.Offset.x : -SkillData.HitBox.Offset.x,
            SkillData.HitBox.Offset.y
        );

        Vector2 size = SkillData.HitBox.Size;
        //Vector2 localSize = Owner.transform.localScale * size;

        Collider2D[] results = Physics2D.OverlapBoxAll(center, size, 0f, LayerMask.GetMask("HitCircle"));

        foreach (Collider2D collider in results)
        {
            Creature target = collider.GetComponentInParent<Creature>();
            if (target != null && target != Owner && _alreadyHit.Contains(target) == false)
            {
                //Debug.Log($"Hit: {target.name} / SkillType: {SkillType} / Animation: {SkillData.AnimName}");
                target.OnDamaged(Owner, this);
                _alreadyHit.Add(target);
                return true;
            }
        }
        return false;
    }

    protected void ResetHitTargets()
    {
        _alreadyHit.Clear();
    }

    protected virtual void OnDrawGizmosSelected()
    {
    #if UNITY_EDITOR
        if (Owner == null || SkillData == null)
            return;

        if (Owner.SkillEnd)
            return;

        if (Owner.NowSkill != this)
            return;

        Vector2 center = (Vector2)transform.position + new Vector2(
            Owner?.LookLeft == true ? SkillData.HitBox.Offset.x : -SkillData.HitBox.Offset.x,
            SkillData.HitBox.Offset.y
        );

        Vector2 size = SkillData.HitBox.Size;
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(center, size);
    #endif
    }
    #endregion
}
