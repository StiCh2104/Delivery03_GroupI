using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public Inventory playerInventory; 
    public Inventory shopInventory;  
    public List<Item> allItems;

    public TextMeshProUGUI playerCoinsText;
    public TextMeshProUGUI shopCoinsText;
    public float playerCoins = 100f;
    public float shopCoins = 100f;
    public float playerHealth = 100f;
    public Slider healthBar;

    public Item selectedItem;
    public bool isPlayerInventory;

    public int minimumInitialPlayerItems = 3;
    public int minimumInitialShopItems = 5;
    private int initialPlayerItems;
    private int initialShopItems;

    public GameObject buyButton;
    public GameObject sellButton;
    public GameObject useButton;

    public InventoryUI playerInventoryUI; 
    public InventoryUI shopInventoryUI;

    void Start()
    {
        SetMaxHealth();
        HideButtons();
        InitializeInventories();
        UpdateUI();
    }

    private void Update()
    {
        if((playerHealth) <= 0)
        {
            Kill();
        }
    }

    void InitializeInventories()
    {
        playerInventory.Clear();
        shopInventory.Clear();

        initialPlayerItems = Random.Range(minimumInitialPlayerItems, 20);
        initialShopItems = Random.Range(minimumInitialShopItems, 20);

        for (int i = 0; i < initialPlayerItems; i++)
        {
            Item randomItem = allItems[Random.Range(0, allItems.Count)];
            playerInventory.AddItem(randomItem);
        }

        for (int i = 0; i < initialShopItems; i++)
        {
            Item randomItem = allItems[Random.Range(0, allItems.Count)];
            shopInventory.AddItem(randomItem);
        }
    }

    public void SetMaxHealth()
    {
        healthBar.maxValue = playerHealth;
        healthBar.value = playerHealth;
    }
    public void SetHealth()
    {
        healthBar.value = playerHealth;
        StartCoroutine(AnimateScale());
    }

    public void HitPlayer()
    {
        playerHealth -= 10;
        SetHealth();
    }
    private IEnumerator AnimateScale()
    {
        healthBar.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
        yield return new WaitForSeconds(0.1f);
        healthBar.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
        yield return new WaitForSeconds(0.1f);
        healthBar.transform.localScale = Vector3.one;
    }

    public void SelectItem(Item item, bool IsPlayerInventory)
    {
        selectedItem = item;
        isPlayerInventory = IsPlayerInventory;
        ShowButtons(item);
    }

    public void BuyItem()
    {
        if (selectedItem != null && playerCoins >= selectedItem.cost && shopInventory.HasItem(selectedItem))
        {
            shopInventory.RemoveItem(selectedItem);
            playerInventory.AddItem(selectedItem);
            playerCoins -= selectedItem.cost;
            shopCoins += selectedItem.cost;
            ResetSelectedItem(shopInventory);
            UpdateUI();
        }
    }

    public void SellItem()
    {
        if (selectedItem != null && playerInventory.HasItem(selectedItem) && shopCoins >= selectedItem.cost)
        {
            playerInventory.RemoveItem(selectedItem);
            shopInventory.AddItem(selectedItem);
            playerCoins += selectedItem.cost;
            shopCoins -= selectedItem.cost;
            ResetSelectedItem(playerInventory);
            UpdateUI();
        }
    }

    public void UseItem()
    {
        if (selectedItem != null && playerInventory.HasItem(selectedItem) && (selectedItem is ItemFood || selectedItem is ItemPotions))
        {
            int restoreValue = (selectedItem is ItemFood food) ? food.LifeRestore : ((ItemPotions)selectedItem).LifeRestore;
            playerHealth = Mathf.Min(playerHealth + restoreValue, 100);
            playerInventory.RemoveItem(selectedItem);
            ResetSelectedItem(playerInventory);
            UpdateUI();
            SetHealth();
        }
    }

    public void TakeDamage()
    {
        playerHealth = Mathf.Max(playerHealth - 10, 0);
        UpdateUI();
    }

    public void ResetSelectedItem(Inventory inventory)
    {
        if(!InventoryHasItem(inventory, selectedItem))
        {
            DeselectItem();
        }
        else
        {
            if (selectedItem.IsStackable)
            {
                HighlightSelectedItem(inventory);
                ShowButtons(selectedItem);
            }
            else 
            {
                DeselectItem();
            }
        }
    }

    void DeselectItem()
    {
        selectedItem = null;
        HideButtons();
    }

    bool InventoryHasItem(Inventory inventory, Item item)
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory.GetSlot(i).HasItem(item))
                return true;
        }
        return false;
    }

    void ShowButtons(Item item)
    {
        buyButton.SetActive(false);
        sellButton.SetActive(false);
        useButton.SetActive(false);

        if (!isPlayerInventory)
            buyButton.SetActive(true);
        if (isPlayerInventory)
        {
            sellButton.SetActive(true);
            if (item.IsConsumable)
                useButton.SetActive(true);
        }
    }

    void HideButtons()
    {
        buyButton.SetActive(false);
        sellButton.SetActive(false);
        useButton.SetActive(false);
    }

    void UpdateUI()
    {
        playerCoinsText.text = "Coins: " + playerCoins;
        shopCoinsText.text = "Coins: " + shopCoins;
    }
    public void DeselectAllItems()
    {
        playerInventoryUI.DeselectAllItems();
        shopInventoryUI.DeselectAllItems();
    }
    private void HighlightSelectedItem(Inventory inventory)
    {
        DeselectAllItems();
        InventoryUI inventoryUI = isPlayerInventory ? playerInventoryUI : shopInventoryUI;
        InventorySlotUI slotUI = inventoryUI.GetSlotUIByItem(selectedItem);
        if (slotUI != null)
        {
            slotUI.HighlightImage.gameObject.SetActive(true);
        }
    }
    private void Kill()
    {

    }
}
