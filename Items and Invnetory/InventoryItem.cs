using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _itemData)
    {
        data = _itemData;
        AddStack();
        // 1000: add to stack
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
