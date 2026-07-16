using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

#region DataID 기준
// 수정 필요

// Creature     십만번대
// - Artifact   100,000 번대
// - Hero       200,000 번대
// - Monster    300,000 번대
// - Boss       500,000 번대

// Skill        만번대
// - None       10,000 번대
// - Sword      20,000 번대
// - Dagger     30,000
// - GreatSword 40,000
// - BattleAxe  50,000
// - Shield     60,000
#endregion

namespace Data
{
    #region CreatureData
    [Serializable]
    public class CreatureData
    {
        public int DataID;
        public string DescriptionTextID;
        public string PrefabLabel;
        public string AnimatorName;

        public int DropItemId;
        public int WoodWeaponId;
        public int FireWeaponId;
        public int EarthWeaponId;
        public int MetalWeaponId;
        public int WaterWeaponId;
        public int BarrierId;

        public float Mass;
        public float MaxHp;
        public float MoveSpeed;
        public float Money;
        public float JumpForce;
        public float CoyoteTime;

        public HitBoxData HitBox;
        public HitBoxData HitCircle;
    }

    #region ArtifactData
    [Serializable]
    public class ArtifactData : CreatureData
    {
        
    }

    [Serializable]
    public class ArtifactDataLoader : ILoader<int, ArtifactData>
    {
        public List<ArtifactData> artifacts = new List<ArtifactData>();
        public Dictionary<int, ArtifactData> MakeDict()
        {
            Dictionary<int, ArtifactData> dict = new Dictionary<int, ArtifactData>();
            foreach (ArtifactData artifact in artifacts)
                dict.Add(artifact.DataID, artifact);
            return dict;
        }
    }
    #endregion

    #region BossData
    [Serializable]
    public class BossData : CreatureData
    {
        public float ReactionSpeed;
    }

    [Serializable]
    public class BossDataLoader : ILoader<int, BossData>
    {
        public List<BossData> bosses = new List<BossData>();
        public Dictionary<int, BossData> MakeDict()
        {
            Dictionary<int, BossData> dict = new Dictionary<int, BossData>();
            foreach (BossData boss in bosses)
                dict.Add(boss.DataID, boss);
            return dict;
        }
    }
    #endregion

    #region HeroData
    [Serializable]
    public class HeroData : CreatureData
    {
        
    }

    [Serializable]
    public class HeroDataLoader : ILoader<int, HeroData>
    {
        public List<HeroData> heroes = new List<HeroData>();
        public Dictionary<int, HeroData> MakeDict()
        {
            Dictionary<int, HeroData> dict = new Dictionary<int, HeroData>();
            foreach (HeroData hero in heroes)
                dict.Add(hero.DataID, hero);
            return dict;
        }
    }
    #endregion

    #region MonsterData
    [Serializable]
    public class MonsterData : CreatureData
    {

    }

    [Serializable]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        public List<MonsterData> monsters = new List<MonsterData>();
        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData monster in monsters)
                dict.Add(monster.DataID, monster);
            return dict;
        }
    }
    #endregion
    #endregion

    #region InteractionData
    [Serializable]
    public class InteractionData
    {
        public int DataID;
        public string DescriptionTextID;
        public string PrefabLabel;
        public string AnimatorName;

        public HitBoxData HitBox;
    }
    

    #region NpcData
    [Serializable]
    public class NpcData : InteractionData
    {

    }

    public class NpcDataLoader : ILoader<int, NpcData>
    {
        public List<NpcData> npcs = new List<NpcData>();
        public Dictionary<int, NpcData> MakeDict()
        {
            Dictionary<int, NpcData> dict = new Dictionary<int, NpcData>();
            foreach (NpcData npc in npcs)
                dict.Add(npc.DataID, npc);
            return dict;
        }
    }
    #endregion
    #endregion

    #region ItemData
    public class ItemData
    {
        public int DataID;
        public string DescriptionTextID;
        public string DescriptionText;
        public string ItemType;
        public string PrefabLabel;
        public string AnimatorName;

        public HitBoxData HitBox;
    }

    public class ItemDataLoader : ILoader<int, ItemData>
    {
        public List<ItemData> items = new List<ItemData>();
        public Dictionary<int, ItemData> MakeDict()
        {
            Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
            foreach (ItemData item in items)
                dict.Add(item.DataID, item);
            return dict;
        }
    }
    #endregion

    #region SkillData
    [Serializable]
    public class SkillData
    {
        public int DataId;
        public string Name;
        public string ClassName;
        public string Description;
        public string IconLabel;
        public string SkillSound;
        public string AnimName;

        public int ComboCount;
        public float CoolTime;
        public float Damage;
        public string HitType;
        public float SuperArmorLevel;
        public float SuperArmorBreakLevel;
        public string AttackType;
        public int ShoveAttck;
        public int StartFrame;
        public int EndFrame;
        public float MoveRange;

        public HitBoxData HitBox;
    }

    [Serializable]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        public List<SkillData> skills = new List<SkillData>();
        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
            foreach (SkillData skill in skills)
                dict.Add(skill.DataId, skill);
            return dict;
        }
    }
    #endregion

    #region WeaponData
    [Serializable]
    public class WeaponData
    {
        public int DataId;
        public ECreatureWeapon Name;
        public string ClassName;
        public string Description;
        public string AnimatorName;

        public int SkillC;

        public CreatureSkillMap SkillShiftMap;
        public CreatureSkillMap SkillZMap;
        public CreatureSkillMap SkillXMap;

        public CreatureSkillMap SkillAMap;
        public CreatureSkillMap SkillSMap;
        public CreatureSkillMap SkillDMap;
        public CreatureSkillMap SkillFMap;

        public int GetSkillId(CreatureSkillMap map, ECreatureType type, ECreatureState state)
        {
            if (map == null)
                return 0;
            
            return map.GetSkillId(type, state);
        }
    }

    [Serializable]
    public class WeaponDataLoader : ILoader<int, WeaponData>
    {
        public List<WeaponData> weapons = new List<WeaponData>();
        public Dictionary<int, WeaponData> MakeDict()
        {
            Dictionary<int, WeaponData> dict = new Dictionary<int, WeaponData>();
            foreach (WeaponData weapon in weapons)
                dict.Add(weapon.DataId, weapon);
            return dict;
        }
    }
    #endregion

    #region CameraData
    [Serializable]
    public class CameraData
    {
        public int DataId;
        public string Name;
        public float Duration;
        public bool ExitUnlock;
        public float VectorX;
        public float VectorY;
    }

    [Serializable]
    public class CameraDataLoader : ILoader<int, CameraData>
    {
        public List<CameraData> cameras = new List<CameraData>();
        public Dictionary<int, CameraData> MakeDict()
        {
            Dictionary<int, CameraData> dict = new Dictionary<int, CameraData>();
            foreach (CameraData camera in cameras)
                dict.Add(camera.DataId, camera);
            return dict;
        }
    }
    #endregion

    #region MapData
    [Serializable]
    public class MapData
    {
        public int DataId;
        public string DescriptionTextID;
        public string PrefabLabel;
        public string Background;
        public List<SpawnData> SpawnPoints;
    }

    [Serializable]
    public class MapDataLoader : ILoader<int, MapData>
    {
        public List<MapData> maps = new List<MapData>();
        public Dictionary<int, MapData> MakeDict()
        {
            Dictionary<int, MapData> dict = new Dictionary<int, MapData>();
            foreach (MapData map in maps)
                dict.Add(map.DataId, map);
            return dict;
        }
    }
    #endregion

    
}