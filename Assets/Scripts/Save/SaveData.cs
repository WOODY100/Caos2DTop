using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int saveVersion = 1;

    // WORLD
    public string sceneName;
    public Vector3 playerPosition;
    public List<string> worldFlags = new();

    // PLAYER
    public int playerLevel;
    public int playerCurrentExp;
    public int playerExpToNextLevel;
    
    // RUNTIME
    public int currentHealth;

    // BASE STATS
    public int baseHealth;
    public int baseAttack;
    public int baseDefense;
    public float baseSpeed;

    // INVENTORY
    public List<string> inventoryItemIDs = new();
    public List<EquippedItemData> equippedItems = new();

    // CURRENCY
    public int coins;

    // WORLD STATE
    public List<string> openedChests = new();
    public List<string> deadEnemies = new();

    //QUESTS
    [Header("Quests")]
    public List<string> completedQuestIDs = new();


    public SaveMetaData meta;
}
