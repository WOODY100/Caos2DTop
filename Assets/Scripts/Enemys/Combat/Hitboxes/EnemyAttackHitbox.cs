using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    private EnemyAttack enemyAttack;
    private HashSet<PlayerStats> hitPlayers = new();

    private void Awake()
    {
        enemyAttack = GetComponentInParent<EnemyAttack>();
    }

    private void OnEnable()
    {
        // 🔹 Nuevo ataque → resetear hits
        hitPlayers.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player == null) return;

        if (hitPlayers.Contains(player))
            return;

        hitPlayers.Add(player);

        int damage = enemyAttack.GetDamage();
        player.TakeDamage(damage);
    }
}