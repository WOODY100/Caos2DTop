using System.Collections;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private EnemyController enemy;
    private EnemyHealth health;

    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int Death = Animator.StringToHash("Death");

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

        animator.SetBool(IsMoving, enemy.IsMoving());

        Vector2 facingDir = FacingToVector(enemy.Facing);

        animator.SetFloat(MoveX, facingDir.x);
        animator.SetFloat(MoveY, facingDir.y);
    }

    private Vector2 FacingToVector(EnemyController.FacingDirection facing)
    {
        switch (facing)
        {
            case EnemyController.FacingDirection.Up:
                return Vector2.up;
            case EnemyController.FacingDirection.Down:
                return Vector2.down;
            case EnemyController.FacingDirection.Left:
                return Vector2.left;
            case EnemyController.FacingDirection.Right:
                return Vector2.right;
            default:
                return Vector2.down;
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
