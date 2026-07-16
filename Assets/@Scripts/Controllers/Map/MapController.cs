using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static Define;

public class MapController : InitBase
{
    public int DataTemplateID { get; set; }
    public Data.MapData MapData { get; private set; }
    public List<SpawnData> SpawnData { get; private set; }
    public Tilemap Tilemap { get; private set; }
    public Bounds Bounds { get; private set; }

    #region Map Component
    BackgroundController Background;
    GameObject[] Fogs;
    #endregion

    public Vector3Int LastHeroSpot { get; set; }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        //ScreenFader = transform.Find("Fader").GetComponent<ScreenFader>();
        Background = transform.Find("Background").GetComponent<BackgroundController>();

        Tilemap = transform.Find("Tilemap").GetComponent<Tilemap>();
        Bounds = transform.Find("Bounds").GetComponent<BoxCollider2D>().bounds;

        Fogs = GameObject.FindGameObjectsWithTag("Fog");
        return true;
    }

    public void SetInfo(int templateID)
    {
        DataTemplateID = templateID;

        MapData = Managers.Data.MapDic[templateID];
        gameObject.name = $"@{MapData.DescriptionTextID}";

        // Background 持失
        Background.SetInfo(EBackgroundType.Background);

        // Creature 持失
        SpawnData = MapData.SpawnPoints;
        foreach (SpawnData sd in SpawnData)
        {
            Vector3 spawnPos = Tilemap.GetCellCenterWorld(sd.Position);
            string spawnPointID = sd.SpawnPointID;

            switch (sd.ObjectType)
            {
                case EObjectType.Monster:
                    if (Managers.Save.DeadCreatures.Contains(spawnPointID))
                        continue;
                    else
                    {
                        Monster creature = Managers.Object.Spawn<Monster>(spawnPos, sd.DataId);
                        creature.SpawnPointID = sd.SpawnPointID;
                    }
                    break;
                case EObjectType.Boss:
                    if (Managers.Save.DeadCreatures.Contains(spawnPointID))
                        continue;
                    else
                    {
                        Boss creature = Managers.Object.Spawn<Boss>(spawnPos, sd.DataId);
                        creature.SpawnPointID = sd.SpawnPointID;
                    }
                    break;
                case EObjectType.Artifact:
                    if (Managers.Save.DeadCreatures.Contains(spawnPointID))
                        continue;
                    else
                    {
                        spawnPos += new Vector3(2, -2, 0);
                        Artifact creature = Managers.Object.Spawn<Artifact>(spawnPos, sd.DataId);
                        creature.SpawnPointID = sd.SpawnPointID;
                    }
                    break;
            }
        }

        foreach (GameObject fog in Fogs)
        {
            fog.GetOrAddComponent<TilemapRenderer>().sortingOrder = SortingLayers.MAP_FOG;
        }
    }
}
