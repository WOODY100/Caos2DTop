using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryManager : MonoBehaviour
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
    public int keys;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        transform.SetParent(null); // fuerza root
        DontDestroyOnLoad(gameObject);
    }


    // ===============================
    // ITEM MANAGEMENT
    // ===============================

    public bool AddItem(ItemData item, int v)
    {
        if (item == null) return false;

        // Monedas
        if (item.type == ItemType.Moneda)
        {
            coins++;
            return true;
        }

        // Llaves
        if (item.type == ItemType.Llave)
        {
            keys++;
            return true;
        }

        // Inventario lleno
        if (items.Count >= inventorySize)
        {
            Debug.Log("Inventario lleno");
            return false;
        }

        items.Add(item);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        if (items.Contains(item))
            items.Remove(item);
        OnInventoryChanged?.Invoke();
    }

    public int GetCoins() => coins;
    public int GetKeys() => keys;

    public int GetAmount(ItemData item)
    {
        if (item == null) return 0;

        if (Stackables.TryGetValue(item, out int amount))
            return amount;

        return 0;
    }
}
