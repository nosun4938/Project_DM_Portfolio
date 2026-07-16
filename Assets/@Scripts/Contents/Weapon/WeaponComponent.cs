using Data;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WeaponComponent : InitBase
{
    #region Roots
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }
    public Transform WeaponRoot { get { return GetRootTransform("@Weapons"); } }
    #endregion

    public List<WeaponBase> WeaponList { get; } = new List<WeaponBase>();

    public WeaponBase Barrier { get; private set; }
    public WeaponBase WoodWeapon { get; private set; }
    public WeaponBase FireWeapon {  get; private set; }
    public WeaponBase EarthWeapon {  get; private set; }
    public WeaponBase MetalWeapon {  get; private set; }
    public WeaponBase WaterWeapon {  get; private set; }

    Creature _owner;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    public void SetInfo(Creature owner, CreatureData creatureData)
    {
        _owner = owner;

        AddWeapon(creatureData.BarrierId, ECreatureWeapon.Barrier);
        AddWeapon(creatureData.WoodWeaponId, ECreatureWeapon.Sword);
        AddWeapon(creatureData.FireWeaponId, ECreatureWeapon.Dagger);
        AddWeapon(creatureData.EarthWeaponId, ECreatureWeapon.Hammer);
        AddWeapon(creatureData.MetalWeaponId, ECreatureWeapon.BattleAxe);
        AddWeapon(creatureData.WaterWeaponId, ECreatureWeapon.Shield);
    }

    public void AddWeapon(int weaponTemplateID, ECreatureWeapon weaponSlot)
    {
        if (weaponTemplateID == 0)
            return;

        if (Managers.Data.WeaponDic.TryGetValue(weaponTemplateID, out var data) == false)
        {
            Debug.LogWarning($"AddSkill Failed: {weaponTemplateID}");
            return;
        }

        WeaponBase weapon = gameObject.AddComponent(Type.GetType(data.ClassName)) as WeaponBase;
        if (weapon == null)
            return;

        weapon.SetInfo(_owner, weaponTemplateID);

        WeaponList.Add(weapon);

        switch (weaponSlot)
        {
            case ECreatureWeapon.Barrier:
                Barrier = weapon;
                break;

            case ECreatureWeapon.Sword:
                WoodWeapon = weapon;
                break;
            case ECreatureWeapon.Dagger:
                FireWeapon = weapon;
                break;
            case ECreatureWeapon.Hammer:
                EarthWeapon = weapon;
                break;
            case ECreatureWeapon.BattleAxe:
                MetalWeapon = weapon;
                break;
            case ECreatureWeapon.Shield:
                WaterWeapon = weapon;
                break;
        }
    }

    public WeaponBase GetCurrentWeapon()
    {
        switch (_owner.CreatureWeapon)
        {
            case ECreatureWeapon.Sword:
                return WoodWeapon;
            case ECreatureWeapon.Dagger:
                return FireWeapon;
            case ECreatureWeapon.Hammer:
                return EarthWeapon;
            case ECreatureWeapon.BattleAxe:
                return MetalWeapon;
            case ECreatureWeapon.Shield:
                return WaterWeapon;

            default:
                return WoodWeapon;
        }
    }
}
