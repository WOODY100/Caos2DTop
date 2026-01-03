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

    public void Equip(ItemData item)
    {
        if (!CanEquip(item)) return;

        equippedItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;

        NotifyChange();
    }

    public void Unequip()
    {
        if (equippedItem == null) return;

        InventoryManager.Instance.AddItem(equippedItem, 1);

        equippedItem = null;
        icon.sprite = null;
        icon.enabled = false;

        NotifyChange();
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
        Unequip();
    }
}
