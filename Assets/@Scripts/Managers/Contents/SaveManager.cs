using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SaveManager
{
    public Dictionary<string, WorldObjectSaveData> WorldObjects = new();

    public HashSet<string> DeadCreatures { get; set; } = new();
    public HashSet<string> GetItems { get; set; } = new();

    public void SetChestOpened(string id)
    {
        if (WorldObjects.TryGetValue(id, out var data))
        {
            data.Active = true;
        }
    }
}
