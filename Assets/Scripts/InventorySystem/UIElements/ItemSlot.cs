using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlot
{
    public Item Item;
    public int Amount;

    public ItemSlot(Item item)
    {
        this.Item = item;
        Amount = 1;
    }

    internal bool HasItem(Item item)
    {
        return (item == Item);
    }

    internal bool CanHold(Item item)
    {
        if (item.IsStackable) return (item == Item);

        return false;
    }

    internal void AddOne()
    {
        Amount++;
    }

    internal void RemoveOne()
    {
        Amount--;
    }

    public bool IsEmpty()
    {
        return Amount < 1;
    }
}


