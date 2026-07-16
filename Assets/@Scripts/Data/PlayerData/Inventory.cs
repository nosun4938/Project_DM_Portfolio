using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Dictionary<int, int> _items;

    public void AddItem(int itemId, int count = 1)
    {
        if (_items.ContainsKey(itemId))
            _items[itemId] += count;
        else
            _items.Add(itemId, count);
    }

    public void RemoveItem(int itemId, int count = 1)
    {
        if (_items.ContainsKey(itemId))
            _items[itemId] -= count;
        else
            return;
    }

    public bool HasItem(int itemId)
    {
        return _items.ContainsKey(itemId);
    }

    public int GetCount(int itemId)
    {
        if (_items.ContainsKey(itemId))
            return _items[itemId];

        return 0;
    }
}
