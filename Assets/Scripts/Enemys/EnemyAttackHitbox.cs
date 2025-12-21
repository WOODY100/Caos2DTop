using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }
    }
}
