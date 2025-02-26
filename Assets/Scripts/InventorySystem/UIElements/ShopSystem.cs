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

    public AudioClip buyAndSell;
    public AudioClip takeDamage;

    private AudioSource audioSource;      

    void Start()
    {
        SetMaxHealth();
        HideButtons();
        InitializeInventories();
        Localizer.OnLanguageChange += UpdateUI;
        StartCoroutine(DelayedUpdateUI());
        SetSound();


    }
    private IEnumerator DelayedUpdateUI()
    {
        yield return null;
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

        initialPlayerItems = Random.Range(minimumInitialPlayerItems, 30);
        initialShopItems = Random.Range(minimumInitialShopItems, 30);

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
            PlaySound(buyAndSell);
            shopInventory.RemoveItem(selectedItem);
            playerInventory.AddItem(selectedItem);
            playerCoins -= selectedItem.cost;
            shopCoins += selectedItem.cost;
            ResetSelectedItem(shopInventory);
            UpdateUI();
            StartCoroutine(AnimateShop());
        }
    }

    public void SellItem()
    {
        if (selectedItem != null && playerInventory.HasItem(selectedItem) && shopCoins >= selectedItem.cost)
        {
            PlaySound(buyAndSell);
            playerInventory.RemoveItem(selectedItem);
            shopInventory.AddItem(selectedItem);
            playerCoins += selectedItem.cost;
            shopCoins -= selectedItem.cost;
            ResetSelectedItem(playerInventory);
            UpdateUI();
            StartCoroutine(AnimatePlayer());
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
        PlaySound(takeDamage);
        UpdateUI();
    }

    private IEnumerator AnimatePlayer()
    {
        RectTransform textRect = playerCoinsText.GetComponentInParent<RectTransform>();
        Color originalColor = playerCoinsText.color;
        Vector3 originalScale = textRect.localScale;
        Vector3 smallScale = originalScale * 0.9f;
        Vector3 bigScale = originalScale * 1.1f;
        playerCoinsText.color = Color.green;
        textRect.localScale = smallScale;
        yield return new WaitForSeconds(0.15f);
        textRect.localScale = bigScale;
        yield return new WaitForSeconds(0.15f);
        textRect.localScale = originalScale;
        playerCoinsText.color = originalColor;
    }
    private IEnumerator AnimateShop()
    {
        RectTransform textRect = shopCoinsText.GetComponentInParent<RectTransform>();
        Color originalColor = shopCoinsText.color;
        Vector3 originalScale = textRect.localScale;
        Vector3 smallScale = originalScale * 0.9f;
        Vector3 bigScale = originalScale * 1.1f;
        shopCoinsText.color = Color.green;
        textRect.localScale = smallScale;
        yield return new WaitForSeconds(0.15f);
        textRect.localScale = bigScale;
        yield return new WaitForSeconds(0.15f);
        textRect.localScale = originalScale;
        shopCoinsText.color = originalColor;
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
    void OnDestroy()
    {
        Localizer.OnLanguageChange -= UpdateUI;
    }
    public void UpdateUI()
    {
        playerCoinsText.text = string.Format(Localizer.Instance.GetText("PlayerCoins"), playerCoins);
        shopCoinsText.text = string.Format(Localizer.Instance.GetText("ShopCoins"), shopCoins);
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
            slotUI.SelectedImage.gameObject.SetActive(true);
        }
    }
    private void Kill()
    {
        Manager.Instance.EndGame();
    }
    public void SetSound()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    public void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            audioSource.PlayOneShot(sound);
        }
        else
        {
            Debug.LogWarning("No se ha asignado audio");
        }
    }

    
}
