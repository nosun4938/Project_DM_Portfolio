using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    public Dictionary<int, Data.ArtifactData> ArtifactDic { get; private set; } = new Dictionary<int, Data.ArtifactData>();
    public Dictionary<int, Data.BossData> BossDic { get; private set; } = new Dictionary<int, Data.BossData>();
    public Dictionary<int, Data.HeroData> HeroDic { get; private set; } = new Dictionary<int, Data.HeroData>();
    public Dictionary<int, Data.MonsterData> MonsterDic { get; private set; } = new Dictionary<int, Data.MonsterData>();
    public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();
    public Dictionary<int, Data.WeaponData> WeaponDic { get; private set; } = new Dictionary<int, Data.WeaponData>();
    public Dictionary<int, Data.CameraData> CameraDic { get; private set; } = new Dictionary<int, Data.CameraData>();
    public Dictionary<int, Data.MapData> MapDic { get; private set; } = new Dictionary<int, Data.MapData>();
    public Dictionary<int, Data.NpcData> NpcDic { get; private set; } = new Dictionary<int, Data.NpcData>();
    public Dictionary<int, Data.ItemData> ItemDic { get; private set; } = new Dictionary<int, Data.ItemData>();

    public void Init()
    {
        ArtifactDic = LoadJson<Data.ArtifactDataLoader, int, Data.ArtifactData>("ArtifactData").MakeDict();
        BossDic = LoadJson<Data.BossDataLoader, int, Data.BossData>("BossData").MakeDict();
        HeroDic = LoadJson<Data.HeroDataLoader, int, Data.HeroData>("HeroData").MakeDict();
        MonsterDic = LoadJson<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData").MakeDict();
        SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData").MakeDict();
        WeaponDic = LoadJson<Data.WeaponDataLoader, int, Data.WeaponData>("WeaponData").MakeDict();
        CameraDic = LoadJson<Data.CameraDataLoader, int, Data.CameraData>("CameraData").MakeDict();
        MapDic = LoadJson<Data.MapDataLoader, int, Data.MapData>("MapData").MakeDict();
        NpcDic = LoadJson<Data.NpcDataLoader, int, Data.NpcData>("NpcData").MakeDict();
        ItemDic = LoadJson<Data.ItemDataLoader, int, Data.ItemData>("ItemData").MakeDict();
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>(path);
        return JsonConvert.DeserializeObject<Loader>(textAsset.text);
    }
}
