using UnityEngine;
using System.Collections.Generic;

public class LevelUpManager : MonoBehaviour
{
    private PlayerExperience playerExp;
    private PlayerStats playerStats;
    private LevelUpUI levelUpUI;

    public static LevelUpManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        playerExp = FindFirstObjectByType<PlayerExperience>();
        playerStats = FindFirstObjectByType<PlayerStats>();

        levelUpUI = FindFirstObjectByType<LevelUpUI>(
            FindObjectsInactive.Include
        );
    }

    private void OnEnable()
    {
        playerExp.OnLevelUp += OnPlayerLevelUp;
    }

    private void OnDisable()
    {
        playerExp.OnLevelUp -= OnPlayerLevelUp;
    }

    private void OnPlayerLevelUp(LevelStats stats, int level)
    {
        if (levelUpUI == null)
        {
            levelUpUI = FindFirstObjectByType<LevelUpUI>(
                FindObjectsInactive.Include
            );

            if (levelUpUI == null)
            {
                Debug.LogError("❌ LevelUpUI no encontrado en la escena");
                return;
            }
        }

        var options = GenerateOptions();
        levelUpUI.Show(options, ApplyOption);
    }

    private List<LevelUpOption> GenerateOptions()
    {
        return new List<LevelUpOption>
        {
            new LevelUpOption
            {
                title = "Vida +20",
                description = "Aumenta la vida máxima",
                health = 20
            },
            new LevelUpOption
            {
                title = "Ataque +5",
                description = "Aumenta el daño",
                attack = 5
            },
            new LevelUpOption
            {
                title = "Velocidad +0.5",
                description = "Te mueves más rápido",
                speed = 0.5f
            }
        };
    }

    private void ApplyOption(LevelUpOption option)
    {
        playerStats.baseHealth += option.health;
        playerStats.baseAttack += option.attack;
        playerStats.baseDefense += option.defense;
        playerStats.baseSpeed += option.speed;

        playerStats.RecalculateStats();
    }
}
