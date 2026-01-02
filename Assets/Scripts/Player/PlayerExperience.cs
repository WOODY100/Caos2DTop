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

    [Header("Level Stat Growth")]
    public List<LevelStats> levelStats = new List<LevelStats>();

    public event Action OnExpChanged;
    public event Action OnLevelUp;

    private PlayerStats stats;

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

        ApplyLevelStats();

        OnLevelUp?.Invoke();
        OnExpChanged?.Invoke();
    }

    private void ApplyLevelStats()
    {
        int index = level - 2; // nivel 2 = índice 0

        if (index < 0 || index >= levelStats.Count)
        {
            Debug.LogWarning("⚠ No hay stats definidos para nivel " + level);
            return;
        }

        LevelStats growth = levelStats[index];

        stats.baseHealth += growth.bonusHealth;
        stats.baseAttack += growth.bonusAttack;
        stats.baseDefense += growth.bonusDefense;
        stats.baseSpeed += growth.bonusSpeed;

        stats.RecalculateStats();
    }
}
