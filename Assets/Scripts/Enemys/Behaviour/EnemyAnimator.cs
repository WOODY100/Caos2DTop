using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private EnemyController enemy;
    private EnemyHealth health;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyController>();
        health = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        // 🔴 SI ESTÁ MUERTO, NO ACTUALIZAR NADA
        if (health != null && health.IsDead)
            return;

        Vector2 dir = enemy.GetMoveDirection();

        animator.SetBool("IsMoving", enemy.IsMoving());

        if (dir != Vector2.zero)
        {
            animator.SetFloat("MoveX", dir.x);
            animator.SetFloat("MoveY", dir.y);
        }
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayHurt()
    {
        animator.SetTrigger("Hurt");
    }

    public void PlayDeath()
    {
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsDead", true);
        StartCoroutine(PlayCorpe());
    }

    IEnumerator PlayCorpe()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("IsDead", false);
    }
}
