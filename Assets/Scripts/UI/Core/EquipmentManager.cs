using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    private PlayerStats playerStats;


    [Header("Equipment Slots")]
    public EquipmentSlotUI helmet;
    public EquipmentSlotUI armor;
    public EquipmentSlotUI legs;
    public EquipmentSlotUI boots;
    public EquipmentSlotUI sword;
    public EquipmentSlotUI necklace;
    public EquipmentSlotUI ring;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        playerStats = FindAnyObjectByType<PlayerStats>();
    }

    public bool Equip(ItemData item)
    {
        if (item == null || !IsEquipable(item))
            return false;

        EquipmentSlotUI slot = GetSlot(item.type);
        if (slot == null || !slot.CanEquip(item))
            return false;

        // 🔁 recuperar item anterior
        ItemData previousItem = slot.Equip(item);

        // 🔄 devolver al inventario si existía
        if (previousItem != null)
            InventoryManager.Instance.AddItem(previousItem, 1);

        playerStats?.RecalculateStats();
        return true;
    }


    public void Unequip(ItemType type)
    {
        EquipmentSlotUI slot = GetSlot(type);
        if (slot == null) return;

        ItemData removedItem = slot.Unequip();

        if (removedItem != null)
            InventoryManager.Instance.AddItem(removedItem, 1);

        playerStats?.RecalculateStats();
    }

    private bool IsEquipable(ItemData item)
    {
        return item.type == ItemType.Helmet
            || item.type == ItemType.Armor
            || item.type == ItemType.Legs
            || item.type == ItemType.Boots
            || item.type == ItemType.Sword
            || item.type == ItemType.Necklace
            || item.type == ItemType.Ring;
    }

    public ItemData[] GetAllEquipped()
    {
        return new ItemData[]
        {
        helmet?.GetItem(),
        armor?.GetItem(),
        legs?.GetItem(),
        boots?.GetItem(),
        sword?.GetItem(),
        necklace?.GetItem(),
        ring?.GetItem()
        };
    }

    private EquipmentSlotUI GetSlot(ItemType type)
    {
        return type switch
        {
            ItemType.Helmet => helmet,
            ItemType.Armor => armor,
            ItemType.Legs => legs,
            ItemType.Boots => boots,
            ItemType.Sword => sword,
            ItemType.Necklace => necklace,
            ItemType.Ring => ring,
            _ => null
        };
    }

    public ItemData GetEquipped(ItemType type)
    {
        return GetSlot(type)?.GetItem();
    }

}
