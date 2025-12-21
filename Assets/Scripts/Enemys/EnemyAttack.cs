using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown = 1.2f;

    [Header("Hitbox")]
    public GameObject hitbox;
    public Vector2 offsetUp;
    public Vector2 offsetDown;
    public Vector2 offsetLeft;
    public Vector2 offsetRight;

    private EnemyAnimator enemyAnimator;
    private EnemyController enemyController;

    private bool canAttack = true;
    private bool isAttacking;

    void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyController = GetComponent<EnemyController>();
    }

    public void TryAttack()
    {
        if (!canAttack || isAttacking) return;

        isAttacking = true;
        canAttack = false;

        enemyController.Stop();
        enemyAnimator.PlayAttack();

        Invoke(nameof(ResetCooldown), attackCooldown);
    }

    void ResetCooldown()
    {
        canAttack = true;
    }

    // 🎞️ Animation Events
    public void EnableHitbox()
    {
        SetHitboxDirection();
        hitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitbox.SetActive(false);
        isAttacking = false;
    }

    void SetHitboxDirection()
    {
        Vector2 dir = enemyController.GetMoveDirection();

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            hitbox.transform.localPosition =
                dir.x > 0 ? offsetRight : offsetLeft;
        }
        else
        {
            hitbox.transform.localPosition =
                dir.y > 0 ? offsetUp : offsetDown;
        }
    }
}
