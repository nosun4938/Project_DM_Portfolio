using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Vector3Int RespawnSpot { get; set; }
    public String RespawnMap { get; set; }
    public Inventory Inventory { get; private set; }
    public int Gold;
    public HashSet<int> LearnedSkills;
    public HashSet<int> AcquiredArtifacts;
}
