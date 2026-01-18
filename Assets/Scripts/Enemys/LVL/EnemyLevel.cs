using UnityEngine;

public class EnemyLevel : MonoBehaviour
{
    [Header("Level Range")]
    public int minLevel = 1;
    public int maxLevel = 10;
    public int levelOffset = 0; // -1, 0, +1

    [Header("Base Stats")]
    public int baseHealth = 20;
    public int baseAttack = 5;
    public int baseExp = 20;
    public float baseSpeed = 1.2f;

    [Header("Scaling")]
    public float healthPerLevel = 1.2f;
    public float attackPerLevel = 1.1f;
    public float expPerLevel = 1.3f;
    public float speedPerLevel = 1.05f;

    [Header("Runtime")]
    public int level;
    public int maxHealth;
    public int attack;
    public int expReward;
    public float speed;

    private void Awake()
    {
        CalculateLevel();
        CalculateStats();
        ApplyToComponents();
    }

    void CalculateLevel()
    {
        PlayerExperience player = FindAnyObjectByType<PlayerExperience>();

        int playerLevel = player != null ? player.level : 1;

        level = Mathf.Clamp(
            playerLevel + levelOffset,
            minLevel,
            maxLevel
        );
    }

    void CalculateStats()
    {
        maxHealth = Mathf.RoundToInt(baseHealth * Mathf.Pow(healthPerLevel, level - 1));
        attack = Mathf.RoundToInt(baseAttack * Mathf.Pow(attackPerLevel, level - 1));
        expReward = Mathf.RoundToInt(baseExp * Mathf.Pow(expPerLevel, level - 1));
        speed = baseSpeed * Mathf.Pow(speedPerLevel, level - 1);
    }

    void ApplyToComponents()
    {
        // Health
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.SetMaxHealth(maxHealth);
        }

        // Experience
        EnemyExperience exp = GetComponent<EnemyExperience>();
        if (exp != null)
        {
            exp.expReward = expReward;
        }
    }

}
