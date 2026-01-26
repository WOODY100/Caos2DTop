using UnityEngine;

[System.Serializable]
public class LootEntry
{
    [Header("Item")]
    public ItemData item;

    [Header("Amount")]
    public int minAmount = 1;
    public int maxAmount = 1;

    [Header("Drop Chance")]
    [Range(0f, 100f)]
    public float dropChance = 100f;

    [Header("Rules")]
    public bool guaranteed = false;

    // 🔹 Valor FINAL generado
    [HideInInspector]
    public int amount;
}
