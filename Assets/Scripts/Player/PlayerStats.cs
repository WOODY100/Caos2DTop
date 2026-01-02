using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static event Action OnStatsChanged;
    public event Action OnHealthChanged;

    [Header("Base Stats")]
    public int baseHealth = 100;
    public int baseAttack = 10;
    public int baseDefense = 5;
    public float baseSpeed = 5f;

    [Header("Final Stats (Runtime)")]
    public int maxHealth;
    public int attack;
    public int defense;
    public float speed;

    [Header("Runtime")]
    public int currentHealth;

    private void Awake()
    {
        RecalculateStats();
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke();
    }

    public void RecalculateStats()
    {
        int oldMaxHealth = maxHealth;

        maxHealth = baseHealth;
        attack = baseAttack;
        defense = baseDefense;
        speed = baseSpeed;

        var equipmentManager = EquipmentManager.Instance;
        if (equipmentManager == null) return;

        foreach (var item in equipmentManager.GetAllEquipped())
        {
            if (item == null) continue;

            maxHealth += item.bonusHealth;
            attack += item.bonusAttack;
            defense += item.bonusDefense;
            speed += item.bonusSpeed;
        }

        // Ajusta la vida actual si cambia el máximo
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 🔔 Notificar a la UI
        OnStatsChanged?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke();
    }
}
