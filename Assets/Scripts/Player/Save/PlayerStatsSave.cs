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
        data.baseHealth = stats.baseHealth;
        data.baseAttack = stats.baseAttack;
        data.baseDefense = stats.baseDefense;
        data.baseSpeed = stats.baseSpeed;

        // 🔹 VIDA ACTUAL
        data.currentHealth = stats.currentHealth;
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
    }
}
