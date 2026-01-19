using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown = 1.2f;
    public bool IsAttacking => isAttacking;

    [Header("Hitboxes")]
    public GameObject hitboxUp;
    public GameObject hitboxDown;
    public GameObject hitboxLeft;
    public GameObject hitboxRight;

    private EnemyAnimator enemyAnimator;
    private EnemyController enemyController;
    private EnemyLevel enemyLevel;

    private bool canAttack = true;
    private bool isAttacking;
    private GameObject currentHitbox;

    void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyController = GetComponent<EnemyController>();
        enemyLevel = GetComponent<EnemyLevel>();

        DisableAllHitboxes();
    }

    public int GetDamage()
    {
        return enemyLevel != null ? enemyLevel.attack : 1;
    }

    public void TryAttack()
    {
        if (!canAttack || isAttacking) return;

        isAttacking = true;
        canAttack = false;

        enemyController.SetCanMove(false);
        enemyController.Stop();

        SelectHitbox();
        enemyAnimator.PlayAttack();

        Invoke(nameof(ResetCooldown), attackCooldown);
    }

    void ResetCooldown()
    {
        canAttack = true;
    }

    void SelectHitbox()
    {
        DisableAllHitboxes();

        Vector2 dir = enemyController.GetMoveDirection();
        if (dir == Vector2.zero)
            dir = Vector2.down;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            currentHitbox = dir.x > 0 ? hitboxRight : hitboxLeft;
        else
            currentHitbox = dir.y > 0 ? hitboxUp : hitboxDown;
    }

    // 🎞️ Animation Events (OFICIALES)
    public void Anim_AttackStart()
    {
        currentHitbox?.SetActive(true);
    }

    public void Anim_AttackEnd()
    {
        enemyController.SetCanMove(true);
        DisableAllHitboxes();
        isAttacking = false;
    }

    void DisableAllHitboxes()
    {
        hitboxUp.SetActive(false);
        hitboxDown.SetActive(false);
        hitboxLeft.SetActive(false);
        hitboxRight.SetActive(false);
    }
}
