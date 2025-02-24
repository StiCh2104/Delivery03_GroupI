using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewInventory", menuName = "Inventory System/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField]
    List<ItemSlot> Slots;
    public int Length => Slots.Count;

    public Action OnInventoryChange;

    public void AddItem(Item item)
    {
        if (Slots == null) Slots = new List<ItemSlot>();

        var slot = GetSlot(item);

        if ((slot != null) && (item.IsStackable))
        {
            slot.AddOne();
        }
        else
        {
            slot = new ItemSlot(item);
            Slots.Add(slot);
        }

        OnInventoryChange?.Invoke();
    }
    public void Clear()
    {
        Slots.Clear();
        OnInventoryChange?.Invoke();
    }
    public int GetItemIndex(Item item)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].Item == item)
                return i;
        }
        return -1;
    }

    public bool HasItem(Item item)
    {
        return GetSlot(item) != null;
    }

    public void RemoveItem(Item item)
    {
        if (Slots == null) return;

        var slot = GetSlot(item);

        if (slot != null)
        {
            slot.RemoveOne();
            if (slot.IsEmpty()) RemoveSlot(slot);
        }

        OnInventoryChange?.Invoke();
    }

    private void RemoveSlot(ItemSlot slot)
    {
        Slots.Remove(slot);
    }

    private ItemSlot GetSlot(Item item)
    {
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].HasItem(item)) return Slots[i];
        }

        return null;
    }

    public ItemSlot GetSlot(int i)
    {
        return Slots[i];
    }
}
