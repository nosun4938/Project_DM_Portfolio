using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpawnTrigger : InitBase
{
    public Vector3Int SpawnSpot;
    public string SpawnMap;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        Managers.Game.Player.RespawnSpot = SpawnSpot;
        Managers.Game.Player.RespawnMap = SpawnMap;
    }
}
