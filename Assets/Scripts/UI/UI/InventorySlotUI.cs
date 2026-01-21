using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour,
    IPointerClickHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private Image arrowUp;
    [SerializeField] private Image arrowDown;
    [SerializeField] private TMP_Text amountText;

    private ItemData item;
    public ItemData Item => item;

    // ─────────────────────────────
    // SETUP
    // ─────────────────────────────
    public void SetItem(ItemData newItem, int amount)
    {
        item = newItem;
        gameObject.SetActive(item != null);

        if (item == null)
        {
            Clear();
            return;
        }

        icon.sprite = item.icon;
        icon.color = Color.white;
        icon.raycastTarget = false;

        amountText.text = amount > 1 ? amount.ToString() : "";
        amountText.gameObject.SetActive(amount > 1);

        UpdateComparison();
    }

    public void Clear()
    {
        item = null;

        icon.sprite = null;
        icon.color = new Color(1, 1, 1, 0);
        icon.raycastTarget = false;

        amountText.text = "";
        amountText.gameObject.SetActive(false);

        if (arrowUp) arrowUp.enabled = false;
        if (arrowDown) arrowDown.enabled = false;
    }

    // ─────────────────────────────
    // CLICK DERECHO → USAR
    // ─────────────────────────────
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (item == null)
            return;

        // Consumible
        if (item is IUsableItem usable)
        {
            bool used = usable.Use();
            if (used)
            {
                InventoryManager.Instance.RemoveItem(item, 1);
            }

            ItemTooltipUI.Instance?.Hide();
            return;
        }

        // Equipable
        if (EquipmentManager.Instance.EquipFromInventory(item))
        {
            ItemTooltipUI.Instance?.Hide();
        }
    }

    // ─────────────────────────────
    // CLICK IZQUIERDO → DRAG
    // ─────────────────────────────
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (Item == null)
            return;

        ItemTooltipUI.Instance?.Hide();
        DragItemUI.Instance.BeginDrag(Item, this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragItemUI.Instance.UpdatePosition(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragItemUI.Instance.EndDrag();
    }

    // ─────────────────────────────
    // TOOLTIP
    // ─────────────────────────────
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item == null)
            return;

        // ❌ NO mostrar tooltip si estamos arrastrando
        if (DragItemUI.Instance != null && DragItemUI.Instance.Item != null)
            return;

        ItemTooltipUI.Instance?.Show(Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipUI.Instance?.Hide();
    }

    private void UpdateComparison()
    {
        // 🔹 SIEMPRE resetear primero
        if (arrowUp) arrowUp.enabled = false;
        if (arrowDown) arrowDown.enabled = false;

        if (!EquipmentManager.Instance || item == null)
            return;

        ItemData equipped = EquipmentManager.Instance.GetEquipped(item.type);

        // 🔹 Si no hay nada equipado → no mostrar flechas
        if (equipped == null)
            return;

        int newScore = item.GetStatScore();
        int equippedScore = equipped.GetStatScore();

        if (newScore > equippedScore)
            arrowUp.enabled = true;
        else if (newScore < equippedScore)
            arrowDown.enabled = true;
        // si son iguales → ninguna flecha
    }
}
