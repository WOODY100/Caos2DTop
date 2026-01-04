using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 30;
    private int currentHealth;

    private EnemyController controller;
    private EnemyAnimator animator;
    private EnemyExperience exp;

    void Awake()
    {
        currentHealth = maxHealth;
        controller = GetComponent<EnemyController>();
        animator = GetComponent<EnemyAnimator>();
        exp = GetComponent<EnemyExperience>();
    }

    public void TakeDamage(int amount, bool isCritical = false)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;

        FloatingTextManager.Instance.ShowDamage(
            amount,
            transform.position + Vector3.up * 0.8f, isCritical
        );

        animator.PlayHurt();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        exp?.GiveExperience();

        FloatingTextManager.Instance.ShowExp(
            exp.expReward,
            transform.position + Vector3.up * 1.2f
        );

        controller.Stop();
        controller.enabled = false;
        animator.PlayDeath();

        Destroy(gameObject, 1.2f);
    }

    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        currentHealth = maxHealth;
    }
}
