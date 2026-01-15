using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [HideInInspector] public bool healthLoadedFromSave;

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

    [Header("Critical Stats")]
    public float critChance = 0.15f;      // 15%
    public float critMultiplier = 1.5f;   // x1.5 daño


    private void Awake()
    {
        EquipmentManager.Instance?.SetPlayerStats(this);

        // ⚠️ NO inicializar stats aquí
        // Esto se hará:
        // - en partida nueva
        // - o después de LoadData()
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

        // 🧠 Ajuste inteligente de vida actual
        if (healthLoadedFromSave)
        {
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            healthLoadedFromSave = false;
        }
        else if (oldMaxHealth > 0)
        {
            float percent = (float)currentHealth / oldMaxHealth;
            currentHealth = Mathf.RoundToInt(maxHealth * percent);
        }
        else
        {
            currentHealth = maxHealth;
        }


        // 🔔 NOTIFICAR CAMBIOS
        OnStatsChanged?.Invoke();
        OnHealthChanged?.Invoke(); // ⬅️ ESTA LÍNEA ES LA CLAVE

        Debug.Log(
    $"[RECALC] FinalStats -> MaxHP:{maxHealth} ATK:{attack} DEF:{defense} SPD:{speed}"
);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        FloatingTextManager.Instance.ShowDamage(
        amount,
        transform.position + Vector3.up
        );

        OnHealthChanged?.Invoke();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        OnHealthChanged?.Invoke();
    }

    public void InitializeNewGame()
    {
        RecalculateStats();
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke();
    }
}
