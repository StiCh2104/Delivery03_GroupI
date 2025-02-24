using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventorySlotUI : MonoBehaviour
{
    public Image Image;
    public Image HighlightImage;
    public TextMeshProUGUI AmountText;
    public Item _item { get; private set; }
    private ShopSystem _shopSystem;
    private bool _isPlayerInventory;
    private bool canAnimate = true;
    public void Initialize(ItemSlot slot, ShopSystem shopSystem, bool IsPlayerInventory)
    {
        Image.sprite = slot.Item.ImageUI;
        Image.SetNativeSize();

        AmountText.text = slot.Amount.ToString();
        AmountText.enabled = slot.Amount > 1;

        _item = slot.Item;

        _shopSystem = shopSystem;
        _isPlayerInventory = IsPlayerInventory;

        HighlightImage.gameObject.SetActive(false);
    }
    public void OnItemClicked()
    {
        _shopSystem.DeselectAllItems();

        _shopSystem.SelectItem(_item, _isPlayerInventory);
        HighlightImage.gameObject.SetActive(true);
        StartCoroutine(AnimateScale());
    }

    public void Deselect()
    {
        HighlightImage.gameObject.SetActive(false); 
    }
    private IEnumerator AnimateScale()
    {
        if (canAnimate)
        {
            canAnimate = false;
            Vector3 originalScale = Image.transform.localScale;
            Vector3 smallScale = originalScale * 0.9f;
            Vector3 bigScale = originalScale * 1.1f;
            Image.transform.localScale = smallScale;
            yield return new WaitForSeconds(0.1f);
            Image.transform.localScale = bigScale;
            yield return new WaitForSeconds(0.1f);
            Image.transform.localScale = originalScale;
        }
        canAnimate = true;
    }
}
