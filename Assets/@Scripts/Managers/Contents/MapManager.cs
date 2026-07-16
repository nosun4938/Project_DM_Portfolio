using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;
using static UnityEditor.Progress;

public class MapManager
{
    public Dictionary<string, MapController> Maps { get; } = new Dictionary<string, MapController>();

    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }
    public Transform MapRoot { get { return GetRootTransform("@Maps"); } }

    public MapController SpawnMap(string templateID)
    {
        GameObject go = Managers.Resource.Instantiate(templateID);
        go.name = templateID;

        MapController map = go.GetComponent<MapController>();
        map.transform.parent = MapRoot;
        Maps.Add(templateID, map);
        map.SetInfo(int.Parse(templateID));

        Managers.Game.NowMap = map;
        return map;
    }

    public void NowMapOff()
    {
        NowCreatureOff();
        Managers.Game.NowMap.gameObject.SetActive(false);
    }

    public void NextMapOn(string name, Vector3Int position)
    {
        MapController map;
        if (Managers.Map.Maps.TryGetValue(name, out map))
        {
            map.gameObject.SetActive(true);
            map.SetInfo(int.Parse(name));
        }
        else
            map = SpawnMap(name);

        Managers.Game.NowMap = map;

        // Player
        Hero player = Managers.Object.Player;

        if (player.IsGrounded)
            player._stateMachine.ChangeState(player._runState);
        else
            player._stateMachine.ChangeState(player._fallState);

        Vector3 mapPos = map.Tilemap.GetCellCenterWorld(position);
        Vector3 nowPos = player.transform.position;
        Vector3 nextPos = new Vector3(mapPos.x, mapPos.y, nowPos.z);
        player.transform.position = nextPos;

        player.Rigidbody.linearVelocityX = 0f;
        if (player.Rigidbody.linearVelocityY > 0f)
            player.Rigidbody.linearVelocityY = 0f;
    }

    public void RespawnMap()
    {
        Vector3Int spot = Managers.Game.Player.RespawnSpot;
        string map = Managers.Game.Player.RespawnMap;
        MapController nowMap = Managers.Game.NowMap;

        if (nowMap != Managers.Map.Maps[map])
        {
            NowMapOff();
            NextMapOn(map, spot);
        }
        else
        {
            NowCreatureOff();
            nowMap.SetInfo(int.Parse(map));
        }
    }

    public void NowCreatureOff()
    {
        foreach (Monster monster in Managers.Object.Monsters.ToList())
            Managers.Object.Despawn(monster);

        foreach (Artifact artifact in Managers.Object.Artifacts.ToList())
            Managers.Object.Despawn(artifact);
        
        Managers.Object.Despawn(Managers.Object.Boss);
            
    }
}
