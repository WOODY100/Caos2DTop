using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipmentManager : MonoBehaviour, ISaveable
{
    private List<EquippedItemData> pendingEquipment;

    private readonly System.Collections.Generic.Dictionary<ItemType, ItemData> equipped =
    new System.Collections.Generic.Dictionary<ItemType, ItemData>();

    public static EquipmentManager Instance;
    private PlayerStats playerStats;


    private Dictionary<ItemType, EquipmentSlotUI> slots =
    new Dictionary<ItemType, EquipmentSlotUI>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
    }

    public bool Equip(ItemData item)
    {
        if (item == null || !IsEquipable(item))
            return false;

        EquipmentSlotUI slot = GetSlot(item.type);
        if (slot == null || !slot.CanEquip(item))
            return false;

        // ❌ Si ya está equipado, no volver a equipar
        if (slot.GetItem() == item)
            return false;

        // 🔁 quitar del inventario
        InventoryManager.Instance.RemoveItem(item);

        // 🔁 equipar y recuperar previo
        ItemData previousItem = slot.Equip(item);

        // 🔥 mantener diccionario sincronizado
        equipped[item.type] = item;

        // 🔄 devolver el anterior al inventario
        if (previousItem != null)
            InventoryManager.Instance.AddItem(previousItem, 1);

        playerStats?.RecalculateStats();
        return true;
    }

    public void Unequip(ItemType type)
    {
        EquipmentSlotUI slot = GetSlot(type);
        if (slot == null || !slot.isActiveAndEnabled)
            return;

        ItemData removedItem = slot.Unequip();

        if (removedItem != null)
        {
            equipped.Remove(type);
            InventoryManager.Instance.AddItem(removedItem, 1);
        }

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
        equipped.GetValueOrDefault(ItemType.Helmet),
        equipped.GetValueOrDefault(ItemType.Armor),
        equipped.GetValueOrDefault(ItemType.Legs),
        equipped.GetValueOrDefault(ItemType.Boots),
        equipped.GetValueOrDefault(ItemType.Sword),
        equipped.GetValueOrDefault(ItemType.Necklace),
        equipped.GetValueOrDefault(ItemType.Ring)
        };
    }

    private EquipmentSlotUI GetSlot(ItemType type)
    {
        slots.TryGetValue(type, out EquipmentSlotUI slot);
        return slot;
    }

    public ItemData GetEquipped(ItemType type)
    {
        return GetSlot(type)?.GetItem();
    }

    public void EquipSilently(ItemData item)
    {
        if (item == null || !IsEquipable(item))
            return;

        EquipmentSlotUI slot = GetSlot(item.type);
        if (slot == null)
            return;

        equipped[item.type] = item;
        slot.Equip(item);
    }

    public IReadOnlyDictionary<ItemType, ItemData> GetEquippedDictionary()
    {
        return equipped;
    }

    public void ClearAllSlots()
    {
        foreach (ItemType type in System.Enum.GetValues(typeof(ItemType)))
        {
            EquipmentSlotUI slot = GetSlot(type);
            if (slot != null)
                slot.Clear();
        }

        equipped.Clear();
    }

    public void SaveData(SaveData data)
    {
        data.equippedItems.Clear();

        foreach (var pair in equipped)
        {
            if (pair.Value == null) continue;

            data.equippedItems.Add(new EquippedItemData
            {
                slotType = pair.Key,
                itemID = pair.Value.itemID
            });
        }
    }

    public void LoadData(SaveData data)
    {
        // Guardamos, NO aplicamos todavía
        pendingEquipment = data.equippedItems;
    }

    public void RegisterSlot(EquipmentSlotUI slot)
    {
        if (slot == null)
            return;

        slots[slot.acceptedType] = slot;
        // ❌ NO aplicar aquí
    }

    public bool EquipFromInventory(ItemData item)
    {
        if (item == null)
            return false;

        EquipmentSlotUI slot = GetSlot(item.type);
        if (slot == null || !slot.isActiveAndEnabled)
            return false;

        // 🔒 quitar del inventario SOLO AQUÍ
        if (!InventoryManager.Instance.RemoveItem(item, 1))
            return false;

        ItemData previous = slot.Equip(item);
        equipped[item.type] = item;

        // 🔁 devolver anterior
        if (previous != null)
            InventoryManager.Instance.AddItem(previous, 1);

        playerStats?.RecalculateStats();
        return true;
    }

    private void TryApplyPendingEquipment()
    {
        if (pendingEquipment == null || pendingEquipment.Count == 0)
            return;

        equipped.Clear();

        foreach (var equippedItem in pendingEquipment)
        {
            ItemData item = ItemDatabase.Instance.GetItem(equippedItem.itemID);
            if (item == null)
                continue;

            EquipSilentlyToSlot(equippedItem.slotType, item);
        }

        pendingEquipment = null;
        playerStats?.RecalculateStats();
    }

    public void NotifySlotUIReady()
    {
        TryApplyPendingEquipment();
    }
    
    private void EquipSilentlyToSlot(ItemType slotType, ItemData item)
    {
        if (item == null)
            return;

        if (!slots.TryGetValue(slotType, out EquipmentSlotUI slot))
            return;

        equipped[slotType] = item;
        slot.Equip(item);
    }

    public void ClearUISlots()
    {
        slots.Clear();
    }
    
    public void RefreshUIFromEquipped()
    {
        foreach (var pair in equipped)
        {
            if (!slots.TryGetValue(pair.Key, out EquipmentSlotUI slot))
                continue;

            if (slot == null || !slot.isActiveAndEnabled)
                continue;

            slot.Equip(pair.Value);
        }
    }
}
