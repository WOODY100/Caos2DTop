using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private PlayerStats stats;

    private void Awake()
    {
        stats = FindAnyObjectByType<PlayerStats>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy == null) return;

        int finalDamage = stats.attack;
        bool isCritical = RollCritical();

        if (isCritical)
        {
            finalDamage = Mathf.RoundToInt(finalDamage * stats.critMultiplier);
        }

        enemy.TakeDamage(finalDamage, isCritical);
    }

    private bool RollCritical()
    {
        return Random.value < stats.critChance;
    }
}
