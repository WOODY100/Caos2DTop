using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour, ISaveable
{
    public static InventoryManager Instance;
    public static event Action OnInventoryChanged;

    [Header("Inventory Settings")]
    public int inventorySize = 25;

    [Header("Inventory Data")]
    public List<ItemData> items = new List<ItemData>();
    public Dictionary<ItemData, int> Stackables = new Dictionary<ItemData, int>();

    [Header("Currency")]
    public int coins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // ===============================
    // ITEM MANAGEMENT
    // ===============================
    public bool AddItem(ItemData item, int amount)
    {
        //Debug.Log($"[Inventory] Agregado: {item.itemName} x{amount}");

        if (item == null || amount <= 0)
            return false;

        // 💰 Monedas
        if (item.type == ItemType.Moneda)
        {
            coins += amount;
            OnInventoryChanged?.Invoke();
            return true;
        }

        // 🗝️ Llaves (NUEVO SISTEMA)
        if (item.type == ItemType.Llave)
        {
            // 👉 Agregar como ítem normal (stackeable)
            if (items.Count + amount > inventorySize)
            {
                Debug.Log("Inventario lleno (no hay espacio suficiente para llaves)");
                return false;
            }

            for (int i = 0; i < amount; i++)
                items.Add(item);

            OnInventoryChanged?.Invoke();
            return true;
        }

        // 🎒 Ítems normales
        // 👉 VALIDAR ESPACIO ANTES
        if (items.Count + amount > inventorySize)
        {
            Debug.Log("Inventario lleno (no hay espacio suficiente)");
            return false;
        }

        // 👉 AGREGAR
        for (int i = 0; i < amount; i++)
        {
            items.Add(item);
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveItem(ItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return false;

        int removed = 0;

        for (int i = items.Count - 1; i >= 0 && removed < amount; i--)
        {
            if (items[i].itemID == item.itemID)
            {
                items.RemoveAt(i);
                removed++;
            }
        }

        if (removed < amount)
            return false;

        OnInventoryChanged?.Invoke();
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        RemoveItem(item, 1);
    }


    public int GetCoins() => coins;

    public int GetAmount(ItemData item)
    {
        if (item == null) return 0;

        if (Stackables.TryGetValue(item, out int amount))
            return amount;

        return 0;
    }

    // ===============================
    // SAVE / LOAD
    // =============================
    public void SaveData(SaveData data)
    {
        data.inventoryItems.Clear();
        data.coins = coins;

        Dictionary<string, int> counts = new Dictionary<string, int>();

        foreach (var item in items)
        {
            if (item == null) continue;

            if (!counts.ContainsKey(item.itemID))
                counts[item.itemID] = 0;

            counts[item.itemID]++;
        }

        foreach (var pair in counts)
        {
            data.inventoryItems.Add(new ItemStackSaveData
            {
                itemID = pair.Key,
                quantity = pair.Value
            });
        }
    }

    public void LoadData(SaveData data)
    {
        items.Clear();
        coins = data.coins;

        if (data.inventoryItems == null)
            return;

        foreach (var saved in data.inventoryItems)
        {
            ItemData item = ItemDatabase.Instance.GetItem(saved.itemID);
            if (item == null)
                continue;

            for (int i = 0; i < saved.quantity; i++)
                items.Add(item);
        }

        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(ItemData item)
    {
        return items.Exists(i => i.itemID == item.itemID);
    }

    public int GetItemCount(ItemData item)
    {
        int count = 0;
        foreach (var i in items)
            if (i == item)
                count++;
        return count;
    }
}
