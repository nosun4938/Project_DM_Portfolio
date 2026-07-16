using Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class SkillComponent : InitBase
{
    public List<SkillBase> ActiveSkills { get; set; } = new List<SkillBase>();

    Creature _owner;

    public Dictionary<SkillKey, SkillBase> _skillMap = new Dictionary<SkillKey, SkillBase>();

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(Creature owner, WeaponData weaponData)
    {
        _owner = owner;

        ECreatureWeapon weaponType = weaponData.Name;

        AddSkill(weaponData.SkillC, weaponType, ECreatureState.Airborne, ESkillSlot.C, 0);

        AddSkillByMap(weaponType, weaponData.SkillShiftMap, ESkillSlot.Shift);
        AddSkillByMap(weaponType, weaponData.SkillZMap, ESkillSlot.Z);
        AddSkillByMap(weaponType, weaponData.SkillXMap, ESkillSlot.X);
        AddSkillByMap(weaponType, weaponData.SkillAMap, ESkillSlot.A);
        AddSkillByMap(weaponType, weaponData.SkillSMap, ESkillSlot.S);
        AddSkillByMap(weaponType, weaponData.SkillDMap, ESkillSlot.D);
        AddSkillByMap(weaponType, weaponData.SkillFMap, ESkillSlot.F);

        /*
        foreach (KeyValuePair<SkillKey, SkillBase> pair in _skillMap)
        {
            Debug.Log($"{pair.Key} => {pair.Value}");
        }*/
    }

    

    void AddSkill(int skillTemplateID, ECreatureWeapon creatureWeapon, ECreatureState creatureState, ESkillSlot skillSlot, int combo)
    {
        if (skillTemplateID == 0)
            return;

        if (Managers.Data.SkillDic.TryGetValue(skillTemplateID, out var data) == false)
        {
            Debug.LogWarning($"AddSkill Failed: {skillTemplateID}");
            return;
        }

        SkillBase skill = gameObject.AddComponent(Type.GetType(data.ClassName)) as SkillBase;
        if (skill == null)
            return;
        skill.SetInfo(_owner, skillTemplateID);

        var key = new SkillKey(creatureWeapon, creatureState, skillSlot, combo);
        _skillMap[key] = skill;
        ActiveSkills.Add(skill);

        if (data.ComboCount > 0)
            AddSkill(skillTemplateID + 1, creatureWeapon, creatureState, skillSlot, combo + 1);
    }

    private int GetSkill(CreatureSkillMap map, ECreatureState state)
    {
        return map.GetSkillId(_owner.CreatureType, state);
    }
    void AddSkillByMap(ECreatureWeapon creatureWeapon, CreatureSkillMap map, ESkillSlot slot)
    {
        AddSkill(GetSkill(map, ECreatureState.Idle), creatureWeapon, ECreatureState.Idle, slot, 0);
        AddSkill(GetSkill(map, ECreatureState.Jump), creatureWeapon, ECreatureState.Jump, slot, 0);
        AddSkill(GetSkill(map, ECreatureState.Dash), creatureWeapon, ECreatureState.Dash, slot, 0);
    }
}
