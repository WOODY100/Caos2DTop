using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator animator;
    private EnemyController enemy;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemy = GetComponent<EnemyController>();
    }

    void Update()
    {
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
        animator.SetBool("IsDead", true);
    }
}
