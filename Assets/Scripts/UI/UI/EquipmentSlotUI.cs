using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Slot Config")]
    public ItemType acceptedType;
    public Image icon;

    private ItemData equippedItem;

    private PlayerStats playerStats;
    private InventoryHUD inventoryHUD;

    private void Awake()
    {
        if (icon != null)
            icon.enabled = false;

        playerStats = Object.FindFirstObjectByType<PlayerStats>();
        inventoryHUD = Object.FindFirstObjectByType<InventoryHUD>();
    }

    public bool CanEquip(ItemData item)
    {
        return item != null && item.type == acceptedType;
    }

    public ItemData Equip(ItemData item)
    {
        if (!CanEquip(item)) return null;

        ItemData previousItem = equippedItem;

        equippedItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;

        NotifyChange();
        return previousItem;
    }

    public ItemData Unequip()
    {
        if (equippedItem == null) return null;

        ItemData oldItem = equippedItem;
        equippedItem = null;

        icon.sprite = null;
        icon.enabled = false;

        NotifyChange();
        return oldItem;
    }

    public ItemData GetItem() => equippedItem;

    private void NotifyChange()
    {
        // 🔄 Recalcula stats
        if (playerStats != null)
            playerStats.RecalculateStats();

        // 🔄 Refresca inventario
        if (inventoryHUD != null)
            inventoryHUD.Refresh();
    }

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
}
