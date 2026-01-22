using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class HotbarSlotUI : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    [Header("UI")]
    public Image icon;
    public TMP_Text amountText;
    public Image cooldownOverlay;
    public TMP_Text keyText;

    private ItemData item;

    [SerializeField] private int slotIndex;
    public int SlotIndex => slotIndex;
    public ItemData Item => item;

    private void Start()
    {
        HotbarManager.Instance?.NotifySlotsUIReady();
    }

    private void Update()
    {
        if (item == null || cooldownOverlay == null)
            return;

        if (!ItemCooldownManager.Instance)
            return;

        float remaining =
            ItemCooldownManager.Instance.GetRemaining(item);
        float total =
            ItemCooldownManager.Instance.GetTotal(item);

        UpdateCooldown(remaining, total);
    }

    public void SetItem(ItemData newItem, int amount)
    {
        item = newItem;

        if (item == null)
        {
            Clear();
            return;
        }

        if (icon)
        {
            icon.sprite = item.icon;
            icon.color = Color.white;
            icon.raycastTarget = false;
        }

        if (amountText)
        {
            amountText.text = amount > 1 ? amount.ToString() : "";
            amountText.gameObject.SetActive(amount > 1);
        }

        // ✅ Mostrar tecla SOLO si hay item
        SetKeyTextVisible(true);
    }

    public void Clear()
    {
        item = null;

        if (icon)
        {
            icon.sprite = null;
            icon.color = new Color(1, 1, 1, 0);
            icon.raycastTarget = false;
        }

        if (amountText)
        {
            amountText.text = "";
            amountText.gameObject.SetActive(false);
        }

        if (cooldownOverlay)
        {
            cooldownOverlay.fillAmount = 0f;
        }

        // ❌ Ocultar tecla cuando no hay item
        SetKeyTextVisible(false);
    }

    public void SafeClear()
    {
        if (!this)
            return;

        if (!gameObject.activeInHierarchy)
            return;

        Clear();
    }

    public void UpdateCooldown(float remaining, float total)
    {
        if (!cooldownOverlay)
            return;

        if (total <= 0f)
        {
            cooldownOverlay.fillAmount = 0f;
            return;
        }

        cooldownOverlay.fillAmount = Mathf.Clamp01(remaining / total);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var drag = DragItemUI.Instance;
        if (drag == null || drag.Item == null)
            return;

        if (drag.Item is not IUsableItem)
            return;

        HotbarManager.Instance.AssignItemToSlot(slotIndex, drag.Item);
        drag.EndDrag();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!this || !gameObject.activeInHierarchy)
            return;

        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (item == null)
            return;

        HotbarManager.Instance.TryUseSlot(slotIndex);
    }

    void SetKeyTextVisible(bool visible)
    {
        if (!keyText) return;

        Color c = keyText.color;
        c.a = visible ? 1f : 0f;
        keyText.color = c;
    }
}