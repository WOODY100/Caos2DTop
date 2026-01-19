using UnityEngine;
using System.Collections.Generic;

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;

    private PlayerExperience playerExp;
    private PlayerStats playerStats;
    private LevelUpUI levelUpUI;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // 🔔 REGISTRO DEL PLAYER (CLAVE)
    public void RegisterPlayer(PlayerExperience exp, PlayerStats stats)
    {
        // Desuscribirse del anterior (por seguridad)
        if (playerExp != null)
            playerExp.OnLevelUp -= OnPlayerLevelUp;

        playerExp = exp;
        playerStats = stats;

        if (playerExp != null)
            playerExp.OnLevelUp += OnPlayerLevelUp;
    }

    public void UnregisterPlayer(PlayerExperience exp)
    {
        if (playerExp == exp)
        {
            playerExp.OnLevelUp -= OnPlayerLevelUp;
            playerExp = null;
            playerStats = null;
        }
    }

    private void OnPlayerLevelUp(LevelStats stats, int level)
    {
        GameStateManager.Instance.SetState(GameState.LevelUp);

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
            title = "Vida +12",
            description = "Aumenta la vida máxima",
            health = 12
        },
        new LevelUpOption
        {
            title = "Ataque +2",
            description = "Aumenta el daño",
            attack = 2
        },
        new LevelUpOption
        {
            title = "Defensa +1",
            description = "Reduce el daño recibido",
            defense = 1
        },
        new LevelUpOption
        {
            title = "Velocidad +0.25",
            description = "Te mueves más rápido",
            speed = 0.25f
        }
    };
    }

    private void ApplyOption(LevelUpOption option)
    {
        if (playerStats == null)
            return;

        playerStats.baseHealth += option.health;
        playerStats.baseAttack += option.attack;
        playerStats.baseDefense += option.defense;
        playerStats.baseSpeed += option.speed;

        playerStats.RecalculateStats();
        GameStateManager.Instance.SetState(GameState.Playing);
    }
}
