using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    private EnemyAttack enemyAttack;

    private void Awake()
    {
        enemyAttack = GetComponentInParent<EnemyAttack>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        int damage = enemyAttack.GetDamage();

        other.GetComponent<PlayerStats>()?.TakeDamage(damage);
    }
}
