using UnityEngine;
using System;

[RequireComponent(typeof(PlayerStats))]
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHealth : MonoBehaviour
{
    public event Action OnPlayerDied;

    private PlayerStats stats;
    private PlayerController controller;
    private Animator animator;
    private Rigidbody2D rb;

    private bool isDead = false;

    void Awake()
    {
        stats = GetComponent<PlayerStats>();
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        stats.OnHealthChanged += CheckDeath;
    }

    void OnDisable()
    {
        stats.OnHealthChanged -= CheckDeath;
    }

    private void CheckDeath()
    {
        if (isDead) return;

        if (stats.currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        // 🔒 Bloquear input y combate
        controller.SetInputEnabled(false);
        controller.SetAttacking(false);

        // 🧊 Detener física
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // 🎬 Animación
        if (animator != null)
            animator.SetTrigger("Die");

        Debug.Log("☠️ PLAYER DEAD");

        OnPlayerDied?.Invoke();
    }

    public bool IsDead()
    {
        return isDead;
    }
}
