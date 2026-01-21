using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour,
    IDropHandler,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Slot Config")]
    public ItemType acceptedType;
    public Image icon;

    private ItemData equippedItem;

    private void Awake()
    {
        EquipmentManager.Instance?.RegisterSlot(this);
        if (icon != null)
            icon.enabled = false;
    }

    private void Start()
    {
        EquipmentManager.Instance?.NotifySlotUIReady();
        RefreshFromEquipment();
    }

    public bool CanEquip(ItemData item)
    {
        return item != null && item.type == acceptedType;
    }

    public ItemData Equip(ItemData item)
    {
        ItemData previous = equippedItem;
        equippedItem = item;

        if (!this || !icon)
            return previous;

        icon.sprite = item.icon;
        icon.enabled = true;

        return previous;
    }

    public ItemData Unequip()
    {
        if (equippedItem == null) return null;

        ItemData old = equippedItem;
        Clear();
        return old;
    }

    public ItemData GetItem() => equippedItem;

    public void Clear()
    {
        equippedItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    // ───────────── INPUT ─────────────

    public void OnPointerClick(PointerEventData eventData)
    {
        if (equippedItem == null) return;

        EquipmentManager.Instance.Unequip(acceptedType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equippedItem == null) return;
        ItemTooltipUI.Instance.Show(equippedItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipUI.Instance.Hide();
    }

    // ───────────── DRAG DROP ─────────────

    public void OnDrop(PointerEventData eventData)
    {
        InventorySlotUI invSlot =
            eventData.pointerDrag?.GetComponent<InventorySlotUI>();

        if (invSlot == null) return;
        if (invSlot.Item == null) return;

        ItemData item = invSlot.Item;

        if (item.type != acceptedType)
            return;

        EquipmentManager.Instance.EquipFromInventory(item);
    }

    public void RefreshFromEquipment()
    {
        ItemData item = EquipmentManager.Instance.GetEquipped(acceptedType);

        if (item != null)
            Equip(item);
        else
            Clear();
    }
}
