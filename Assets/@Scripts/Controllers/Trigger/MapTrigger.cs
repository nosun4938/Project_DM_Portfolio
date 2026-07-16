using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTrigger : InitBase
{
    string nextMap;
    int nextMapID;
    public Vector3Int nextMapPosition;
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        nextMap = gameObject.name;

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == false)
            return;

        //Managers.Game.NowMC.ScreenFader.FadeOut();

        Managers.Map.NowMapOff();
        Managers.Map.NextMapOn(nextMap, nextMapPosition);
    }
}
