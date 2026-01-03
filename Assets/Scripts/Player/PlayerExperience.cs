using UnityEngine;
using System;
using System.Collections.Generic;

public class PlayerExperience : MonoBehaviour
{
    [Header("Level")]
    public int level = 1;
    [SerializeField] private LevelUpUI levelUpUI;

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

        int index = level - 2;

        if (index >= 0 && index < levelStats.Count)
        {
            LevelStats gained = levelStats[index];
            ApplyLevelStats(gained);

            levelUpUI?.Show(gained, level);
        }
        Debug.Log($"LEVEL {level} → usando LevelStats[{index}]");

        OnLevelUp?.Invoke();
        OnExpChanged?.Invoke();
    }


    private void ApplyLevelStats(LevelStats growth)
    {
        stats.baseHealth += growth.bonusHealth;
        stats.baseAttack += growth.bonusAttack;
        stats.baseDefense += growth.bonusDefense;
        stats.baseSpeed += growth.bonusSpeed;

        stats.RecalculateStats();
        stats.currentHealth = stats.maxHealth;
    }
}
