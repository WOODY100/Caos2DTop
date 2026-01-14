using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public SaveMetaData meta;

    // ======================
    // WORLD
    // ======================
    public string sceneName;
    public Vector3 playerPosition;

    // ======================
    // INVENTORY
    // ======================
    public List<string> inventoryItemIDs = new();

    // ======================
    // EQUIPMENT
    // ======================
    public List<EquippedItemData> equippedItems = new();

    // ======================
    // CURRENCY
    // ======================
    public int coins;
    public int keys;
}
