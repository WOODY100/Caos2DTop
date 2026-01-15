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
        data.inventoryItemIDs.Clear();

        foreach (var item in items)
        {
            if (item != null)
                data.inventoryItemIDs.Add(item.itemID);
        }

        data.coins = coins;
    }

    public void LoadData(SaveData data)
    {
        items.Clear();
        coins = data.coins;

        foreach (string id in data.inventoryItemIDs)
        {
            ItemData item = ItemDatabase.Instance.GetItem(id);
            if (item != null)
                AddItem(item, 1);
        }

        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(ItemData item)
    {
        return items.Exists(i => i.itemID == item.itemID);
    }
}
