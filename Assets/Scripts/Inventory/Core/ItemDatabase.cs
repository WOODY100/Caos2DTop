using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance;

    [Header("Items")]
    [SerializeField] private List<ItemData> items;

    private Dictionary<string, ItemData> lookup;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        lookup = new Dictionary<string, ItemData>();

        foreach (var item in items)
        {
            if (item == null || string.IsNullOrEmpty(item.itemID))
            {
                Debug.LogError("ItemDatabase: Item inválido o sin ID");
                continue;
            }

            if (lookup.ContainsKey(item.itemID))
            {
                Debug.LogError($"ItemDatabase: ID duplicado → {item.itemID}");
                continue;
            }

            lookup.Add(item.itemID, item);
        }
    }

    public ItemData GetItem(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        lookup.TryGetValue(id, out ItemData item);
        return item;
    }
}
