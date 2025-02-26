using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image Image;
    public Image SelectedImage;
    public Image HighlightImage;
    public TextMeshProUGUI AmountText;

    private string ItemName;
    private float ItemValue;
    public Item _item { get; private set; }
    private ShopSystem _shopSystem;
    private bool _isPlayerInventory;
    private bool canAnimate = true;
    private Transform originalParent;
    private InventoryUI originalInventoryUI;

    private Canvas _canvas;
    private GraphicRaycaster _raycaster;
    private Transform _parent;

    public GameObject popUpPrefab; 
    private GameObject popUpInstance;

    public AudioClip pickUp;
    public AudioSource audioSource;

    public void Initialize(ItemSlot slot, ShopSystem shopSystem, bool IsPlayerInventory, InventoryUI inventoryUI)
    {
        _item = slot.Item;
        ItemName = _item.name;
        ItemValue = _item.cost;

        Image.sprite = slot.Item.ImageUI;
        Image.SetNativeSize();

        AmountText.text = slot.Amount.ToString();
        AmountText.enabled = slot.Amount > 1;

        _shopSystem = shopSystem;
        _isPlayerInventory = IsPlayerInventory;

        HighlightImage.gameObject.SetActive(false);
        SelectedImage.gameObject.SetActive(false);
        originalInventoryUI = inventoryUI;

        if (audioSource ==  null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    public void OnItemClicked()
    {
        _shopSystem.DeselectAllItems();
        _shopSystem.SelectItem(_item, _isPlayerInventory);
        SelectedImage.gameObject.SetActive(true);
        StartCoroutine(AnimateScale());
        PlaySound(pickUp);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        _parent = transform.parent;

        if (!_canvas)
        {
            _canvas = GetComponentInParent<Canvas>();
            _raycaster = _canvas.GetComponent<GraphicRaycaster>();
        }

        transform.SetParent(_canvas.transform, true);
        transform.SetAsLastSibling();
        transform.localScale *= 1.3f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.localPosition += new Vector3(eventData.delta.x, eventData.delta.y, 0);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

        if (hitCollider != null)
        {
            InventoryUI targetInventoryUI = hitCollider.GetComponentInParent<InventoryUI>();

            if (targetInventoryUI != null)
            {
                if (targetInventoryUI != originalInventoryUI)
                {
                    _shopSystem.SelectItem(_item, _isPlayerInventory);

                    if (_isPlayerInventory)
                        _shopSystem.SellItem();
                    else
                        _shopSystem.BuyItem();
                }
            }        
        }
        transform.SetParent(_parent.transform);
        transform.localPosition = Vector3.zero;
        HideInfoText();
        transform.localScale /= 1.3f;
    }

    public void SetItem(Item item)
    {
        _item = item;
        Image.sprite = item ? item.ImageUI : null;
    }
    public void Deselect()
    {
        SelectedImage.gameObject.SetActive(false);
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
    public void ShowInfoText()
    {
        if (popUpInstance != null) return;
        Canvas mainCanvas = FindObjectOfType<Canvas>();
        popUpInstance = Instantiate(popUpPrefab, mainCanvas.transform);
        PopUp popUpScript = popUpInstance.GetComponent<PopUp>();
        popUpScript.NameText.text = ItemName;
        popUpScript.ValueText.text = "Value: " + ItemValue;
        Canvas popUpCanvas = popUpInstance.GetComponent<Canvas>();
        if (popUpCanvas == null)
        {
            popUpCanvas = popUpInstance.AddComponent<Canvas>();
            popUpInstance.AddComponent<GraphicRaycaster>();
        }
        popUpCanvas.overrideSorting = true;
        popUpCanvas.sortingOrder = 999;
        popUpInstance.transform.SetAsLastSibling();
        RectTransform popUpRect = popUpInstance.GetComponent<RectTransform>();
        RectTransform slotRect = GetComponent<RectTransform>();
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.GetComponent<RectTransform>(),
            RectTransformUtility.WorldToScreenPoint(mainCanvas.worldCamera, slotRect.position),
            mainCanvas.worldCamera, out localPosition);

        popUpRect.localPosition = localPosition + new Vector2(0, -50);

        HighlightImage.gameObject.SetActive(true);
    }



    public void HideInfoText()
    {
        if (popUpInstance != null)
        {
            Destroy(popUpInstance);
            popUpInstance = null;
        }
        HighlightImage.gameObject.SetActive(false);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
