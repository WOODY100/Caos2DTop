using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    [SerializeField] private EnemyAnimator enemyAnimator;
    [SerializeField] private EnemyAttack enemyAttack;

    void Awake()
    {
        if (enemyAnimator == null)
            enemyAnimator = GetComponentInParent<EnemyAnimator>();

        if (enemyAttack == null)
            enemyAttack = GetComponentInParent<EnemyAttack>();

        if (enemyAnimator == null)
            Debug.LogError("[EnemyAnimationEvents] EnemyAnimator NOT FOUND", this);

        if (enemyAttack == null)
            Debug.LogError("[EnemyAnimationEvents] EnemyAttack NOT FOUND", this);
    }

    // =====================
    // ⚔️ ATTACK EVENTS
    // =====================
    public void Anim_AttackStart()
    {
        if (enemyAttack == null) return;
        enemyAttack.Anim_AttackStart();
    }

    public void Anim_AttackEnd()
    {
        if (enemyAttack == null) return;
        enemyAttack.Anim_AttackEnd();
    }

    // =====================
    // 💀 DEATH EVENT
    // =====================
    public void Anim_DeathFinished()
    {
        if (enemyAnimator == null) return;
        enemyAnimator.Anim_DeathFinished();
    }
}
