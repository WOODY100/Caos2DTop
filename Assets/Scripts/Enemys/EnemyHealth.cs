using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 5;
    private int currentHealth;

    private EnemyController controller;
    private EnemyAnimator animator;

    void Awake()
    {
        currentHealth = maxHealth;
        controller = GetComponent<EnemyController>();
        animator = GetComponent<EnemyAnimator>();
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        animator.PlayHurt();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        controller.Stop();
        controller.enabled = false;
        animator.PlayDeath();
        Destroy(gameObject, 1.2f); // tiempo de animación
    }
}
