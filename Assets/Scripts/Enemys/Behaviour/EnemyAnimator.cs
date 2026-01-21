using System.Collections;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private EnemyController enemy;
    private EnemyHealth health;

    void Awake()
    {
        enemy = GetComponent<EnemyController>();
        health = GetComponent<EnemyHealth>();

        // 🔑 Auto-buscar si no está asignado
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (animator == null)
            Debug.LogError($"[EnemyAnimator] No Animator found on {name}");
    }

    void Update()
    {
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
    }
    public void Anim_DeathFinished()
    {
        animator.SetTrigger("DeathFinished");
    }

    public void ResetToIdle()
    {
        if (animator == null) return;

        animator.Rebind();          // 🔑 Limpia TODOS los parámetros y estados
        animator.Update(0f);        // Fuerza actualización inmediata

        animator.SetBool("IsDead", false);
        animator.SetBool("IsMoving", false);

        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Hurt");
        animator.ResetTrigger("DeathFinished");
    }
}
