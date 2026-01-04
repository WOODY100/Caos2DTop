using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerExperience : MonoBehaviour
{
    [Header("Level")]
    public int level = 1;

    [Header("Experience")]
    public int currentExp = 0;
    public int expToNextLevel = 100;

    [Header("Growth")]
    public float expMultiplier = 1.5f;

    public event Action OnExpChanged;
    public event Action<LevelStats, int> OnLevelUp;

    private PlayerStats stats;

    // 🔹 Stats pendientes de aplicar
    private LevelStats pendingStats;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        OnExpChanged?.Invoke();

        while (currentExp >= expToNextLevel)
            LevelUp();
    }

    private void LevelUp()
    {
        currentExp -= expToNextLevel;
        level++;

        expToNextLevel = Mathf.RoundToInt(expToNextLevel * expMultiplier);

        int index = level - 2;

        // 🔔 Avisar al sistema de elecciones
        OnLevelUp?.Invoke(pendingStats, level);

        Debug.Log($"LEVEL {level} → esperando elección");

        OnExpChanged?.Invoke();
    }

    // 🔹 ESTE método se llamará cuando el jugador elija
    public void ApplyChosenStats(LevelStats chosenStats)
    {
        if (chosenStats == null) return;

        stats.baseHealth += chosenStats.bonusHealth;
        stats.baseAttack += chosenStats.bonusAttack;
        stats.baseDefense += chosenStats.bonusDefense;
        stats.baseSpeed += chosenStats.bonusSpeed;

        stats.RecalculateStats();
        stats.currentHealth = stats.maxHealth;

        pendingStats = null;
    }
}
