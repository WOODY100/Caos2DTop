using System.Collections.Generic;
using UnityEngine;
using System;


public class WorldStateManager : MonoBehaviour, ISaveable
{
    public static event Action OnWorldStateChanged;
    public static event Action<string> OnFlagSet;
    public static WorldStateManager Instance;

    // ======================
    // CHESTS
    // ======================
    private HashSet<string> openedChests = new();

    // ======================
    // ENEMIES
    // ======================
    private HashSet<string> deadEnemies = new();

    // ======================
    // WORLD FLAGS (GENÉRICO)
    // ======================
    private HashSet<string> worldFlags = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // ======================
    // CHESTS
    // ======================

    public void MarkChestOpened(string chestID)
    {
        if (string.IsNullOrEmpty(chestID))
            return;

        openedChests.Add(chestID);
    }

    public bool IsChestOpened(string chestID)
    {
        return openedChests.Contains(chestID);
    }

    // ======================
    // ENEMIES
    // ======================

    public void MarkEnemyDead(string enemyID)
    {
        if (string.IsNullOrEmpty(enemyID))
            return;

        deadEnemies.Add(enemyID);
    }

    public bool IsEnemyDead(string enemyID)
    {
        return deadEnemies.Contains(enemyID);
    }

    // ======================
    // FLAGS (NUEVO)
    // ======================

    public void SetFlag(string flagID)
    {
        if (string.IsNullOrEmpty(flagID))
            return;

        if (worldFlags.Add(flagID))
        {
            OnWorldStateChanged?.Invoke();
            OnFlagSet?.Invoke(flagID);
        }
    }

    public bool HasFlag(string flagID)
    {
        if (string.IsNullOrEmpty(flagID))
            return false;

        return worldFlags.Contains(flagID);
    }

    public void ClearFlag(string flagID)
    {
        if (string.IsNullOrEmpty(flagID))
            return;

        worldFlags.Remove(flagID);
    }

    // ======================
    // SAVE / LOAD
    // ======================

    public void SaveData(SaveData data)
    {
        data.openedChests.Clear();
        data.openedChests.AddRange(openedChests);

        data.deadEnemies.Clear();
        data.deadEnemies.AddRange(deadEnemies);

        data.worldFlags.Clear();
        data.worldFlags.AddRange(worldFlags);
    }

    public void LoadData(SaveData data)
    {
        openedChests.Clear();
        deadEnemies.Clear();
        worldFlags.Clear();

        if (data.openedChests != null)
            foreach (var id in data.openedChests)
                openedChests.Add(id);

        if (data.deadEnemies != null)
            foreach (var id in data.deadEnemies)
                deadEnemies.Add(id);

        if (data.worldFlags != null)
            foreach (var flag in data.worldFlags)
                worldFlags.Add(flag);

        OnWorldStateChanged?.Invoke();
    }

    #if UNITY_EDITOR
        public IEnumerable<string> GetAllFlags() => worldFlags;
    #endif
}
