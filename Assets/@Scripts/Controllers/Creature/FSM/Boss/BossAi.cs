using JetBrains.Annotations;
using NUnit.Framework.Internal.Commands;
using UnityEngine;
using static Define;

public class BossAi
{
    public EMonsterAIState CurrentState;
    protected Boss Owner;

    //private float lastChangeTime;

    public BossAi(Boss boss)
    {
        Owner = boss;
    }

    #region Distance
    public ESkillSlot DistanceSlot()
    {
        ESkillSlot slot = ESkillSlot.None;
        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.BattleAxe:
                slot = BattleAxeDistance();
                break;

        }

        return slot;
    }

    public ESkillSlot BattleAxeDistance()
    {
        ESkillSlot slot = ESkillSlot.None;

        float absDistance = Mathf.Abs(Owner.TargetDistance);
        float dice = Random.Range(0, 10);

        if (absDistance < 12)
        {
            Debug.Log("Distance 0 ~ 3");
            if (dice < 6)       slot = ESkillSlot.Z;
            else if (dice < 8)  slot = ESkillSlot.X;
            else                slot = ESkillSlot.Shift;
        }
        else if (absDistance < 20)
        {
            Debug.Log("Distance 3 ~ 5");
            if (dice < 4)       slot = ESkillSlot.A;
            else if (dice < 8)  slot = ESkillSlot.X;
            else                slot = ESkillSlot.S;
        }
        else if (absDistance < 48)
        {
            Debug.Log("Distance 5 ~ 12");
            slot = ESkillSlot.None;
        }
        else if (absDistance < 56)
        {
            Debug.Log("Distance 12 ~ 14");
            slot = ESkillSlot.D;
        }
        else if (absDistance < 64)
        {
            Debug.Log("Distance 14 ~ 16");
            slot = ESkillSlot.None;
        }
        else if (absDistance >= 64)
        {
            Debug.Log("Distance 16 Over");
            if (dice < 5)       slot = ESkillSlot.None;
            else                slot = ESkillSlot.F;
        }

        return slot;
    }
    #endregion

    #region Skill Hit
    public ESkillSlot SkillHitSlot(ESkillSlot nowSlot)
    {
        ESkillSlot slot = ESkillSlot.None;

        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.BattleAxe:
                slot = BattleAxeSkillHit(nowSlot);
                break;

            default:
                slot = ESkillSlot.None;
                break;
        }

        return slot;
    }

    public ESkillSlot BattleAxeSkillHit(ESkillSlot nowSlot)
    {
        Debug.Log("Axe Hit");
        ESkillSlot slot = ESkillSlot.None;

        float dice = Random.Range(0, 10);
        if (nowSlot == ESkillSlot.Z)
        {
            if (dice < 5)   slot = ESkillSlot.X;
            else            slot = ESkillSlot.A;
        }

        if (nowSlot == ESkillSlot.X || nowSlot == ESkillSlot.D)
        {
            if (dice < 5)   slot = ESkillSlot.A;
            else            slot = ESkillSlot.S;
        }

        if (nowSlot == ESkillSlot.A)
            slot = ESkillSlot.S;

        if (nowSlot == ESkillSlot.S)
        {
            if (dice < 9)   slot = ESkillSlot.None;
            else            slot = ESkillSlot.Shift;
        }

        return slot;
    }

    #endregion

    #region Skill Miss
    public ESkillSlot SkillMissSlot()
    {
        ESkillSlot slot = ESkillSlot.None;
        switch (Owner.CreatureWeapon)
        {
            case ECreatureWeapon.BattleAxe:
                slot = BattleAxeSkillMiss();
                break;

            default:
                slot = ESkillSlot.None;
                break;
        }

        return slot;
    }

    public ESkillSlot BattleAxeSkillMiss()
    {
        ESkillSlot slot = ESkillSlot.None;

        Debug.Log("Axe Miss");
        float dice = Random.Range(0, 10);

        if (dice < 3)
            slot = ESkillSlot.Shift;
        else
            slot = ESkillSlot.None;
        
        return slot;
    }
    #endregion
}
