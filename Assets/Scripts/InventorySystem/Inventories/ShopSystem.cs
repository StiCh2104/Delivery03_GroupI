using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public List<Item> playerInventory = new List<Item>();
    public List<Item> shopInventory = new List<Item>();
    public Text playerCoinsText;
    public Text playerLifeText;
    public int playerCoins = 100;
    public int playerLife = 100;
    public Slider lifeBar;
    public Item selectedItem;

    void Start()
    {
        UpdateUI();
    }

    public void SelectItem(Item item)
    {
        selectedItem = item;
    }

    public void BuyItem()
    {
        if (selectedItem != null && playerCoins >= selectedItem.cost && shopInventory.Contains(selectedItem))
        {
            playerInventory.Add(selectedItem);
            shopInventory.Remove(selectedItem);
            //playerCoins -= selectedItem.cost;
            UpdateUI();
        }
    }

    public void SellItem()
    {
        if (selectedItem != null && playerInventory.Contains(selectedItem))
        {
            shopInventory.Add(selectedItem);
            playerInventory.Remove(selectedItem);
            //playerCoins += selectedItem.cost;
            UpdateUI();
        }
    }

    public void UseItem()
    {
        /*if (selectedItem != null && playerInventory.Contains(selectedItem) && selectedItem.lifeRestore > 0)
        {
            playerLife = Mathf.Min(playerLife + selectedItem.lifeRestore, 100);
            playerInventory.Remove(selectedItem);
            UpdateUI();
        }*/
    }

    public void TakeDamage()
    {
        playerLife = Mathf.Max(playerLife - 10, 0);
        UpdateUI();
    }

    void UpdateUI()
    {
        playerCoinsText.text = "Coins: " + playerCoins;
        playerLifeText.text = "Life: " + playerLife;
        lifeBar.value = playerLife / 100f;
    }
}
