using Data;
using System;
using UnityEngine;
using static Define;

public class WeaponBase : InitBase
{
    public ECreatureWeapon WeaponType { get; protected set; }
    public SkillComponent Skills { get; protected set; }
    public Data.WeaponData WeaponData { get; private set; }
    
    public Creature Owner { get; protected set; }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public virtual void SetInfo(Creature owner, int weaponTemplateID)
    {
        WeaponData = Managers.Data.WeaponDic[weaponTemplateID];
        Owner = owner;

        // WeaponType
        WeaponType = WeaponData.Name;

        // Skill
        Skills = gameObject.GetOrAddComponent<SkillComponent>();
        Skills.SetInfo(owner, WeaponData);
    }
}
