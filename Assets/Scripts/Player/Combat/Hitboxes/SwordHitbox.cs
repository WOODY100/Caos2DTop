using System.Collections.Generic;
using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    private PlayerStats stats;

    [SerializeField] private LayerMask enemyLayer;

    private HashSet<EnemyHealth> hitEnemies = new();
    private Collider2D hitboxCollider;

    private readonly Collider2D[] results = new Collider2D[8];
    private ContactFilter2D filter;

    private void Awake()
    {
        stats = GetComponentInParent<PlayerStats>();
        hitboxCollider = GetComponent<Collider2D>();

        filter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = enemyLayer,
            useTriggers = true
        };
    }

    private void OnEnable()
    {
        hitEnemies.Clear();
    }

    // 🎬 Animation Event
    public void ApplyDamage()
    {
        int count = hitboxCollider.Overlap(filter, results);

        for (int i = 0; i < count; i++)
        {
            Collider2D col = results[i];
            if (col == null) continue;

            EnemyHealth enemy = col.GetComponent<EnemyHealth>();
            if (enemy == null) continue;

            if (hitEnemies.Contains(enemy))
                continue;

            hitEnemies.Add(enemy);

            int finalDamage = CalculateDamage(out bool isCritical);
            enemy.TakeDamage(finalDamage, isCritical);
        }
    }

    private int CalculateDamage(out bool isCritical)
    {
        isCritical = Random.value < stats.critChance;

        float baseDamage = stats.attack;
        if (isCritical)
            baseDamage *= stats.critMultiplier;

        baseDamage *= Random.Range(0.9f, 1.1f);
        return Mathf.RoundToInt(baseDamage);
    }
}