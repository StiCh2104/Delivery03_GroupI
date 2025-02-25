using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory Inventory;
    public InventorySlotUI SlotPrefab;
    public ShopSystem _shopSystem;
    public bool IsPlayerInventory;

    private List<GameObject> _shownObjects = new List<GameObject>();

    void Start()
    {
        FillInventory(Inventory);
    }

    private void OnEnable()
    {
        Inventory.OnInventoryChange += UpdateInventory;
    }

    private void OnDisable()
    {
        Inventory.OnInventoryChange -= UpdateInventory;
    }

    private void UpdateInventory()
    {
        ClearInventory();
        FillInventory(Inventory);
    }

    private void ClearInventory()
    {
        foreach (var item in _shownObjects)
        {
            if (item) Destroy(item);
        }
        _shownObjects.Clear();
    }

    private void FillInventory(Inventory inventory)
    {
        if (_shownObjects.Count > 0) ClearInventory();

        for (int i = 0; i < inventory.Length; i++)
        {
            _shownObjects.Add(AddSlot(inventory.GetSlot(i)));
        }
    }

    private GameObject AddSlot(ItemSlot inventorySlot)
    {
        var element = Instantiate(SlotPrefab, Vector3.zero, Quaternion.identity, transform);
        element.Initialize(inventorySlot, _shopSystem, IsPlayerInventory, this);
        return element.gameObject;
    }

    public void UseItem(Item item)
    {
        Inventory.RemoveItem(item);
    }

    public void DeselectAllItems()
    {
        foreach (var slot in _shownObjects)
        {
            var slotUI = slot.GetComponent<InventorySlotUI>();
            if (slotUI)
            {
                slotUI.SelectedImage.gameObject.SetActive(false);
            }
        }
    }

    public InventorySlotUI GetSlotUIByItem(Item item)
    {
        foreach (var slot in _shownObjects)
        {
            var slotUI = slot.GetComponent<InventorySlotUI>();
            if (slotUI && slotUI._item == item)
            {
                return slotUI;
            }
        }
        return null;
    }
}
