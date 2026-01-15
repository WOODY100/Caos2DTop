using UnityEngine;

public class PlayerStatsSave : MonoBehaviour, ISaveable
{
    private PlayerStats stats;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    public void SaveData(SaveData data)
    {
        Debug.Log(
        $"[SAVE] BaseStats -> HP:{stats.baseHealth} ATK:{stats.baseAttack} DEF:{stats.baseDefense} SPD:{stats.baseSpeed}"
    );
        data.baseHealth = stats.baseHealth;
        data.baseAttack = stats.baseAttack;
        data.baseDefense = stats.baseDefense;
        data.baseSpeed = stats.baseSpeed;

        // 🔹 VIDA ACTUAL
        data.currentHealth = stats.currentHealth;
        Debug.Log($"[SAVE] HP {stats.currentHealth}/{stats.maxHealth}");
    }

    public void LoadData(SaveData data)
    {
        stats.baseHealth = data.baseHealth;
        stats.baseAttack = data.baseAttack;
        stats.baseDefense = data.baseDefense;
        stats.baseSpeed = data.baseSpeed;
        // 🔹 VIDA ACTUAL
        stats.currentHealth = data.currentHealth;
        stats.healthLoadedFromSave = true;

        Debug.Log($"[LOAD] HP {stats.currentHealth}");
    }
}
