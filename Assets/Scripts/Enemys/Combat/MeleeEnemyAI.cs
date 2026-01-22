using UnityEngine;

[RequireComponent(typeof(EnemyAttack))]
public class MeleeEnemyAI : EnemyAIBase
{
    private EnemyAttack attack;

    protected override void Awake()
    {
        base.Awake();
        attack = GetComponent<EnemyAttack>();
    }

    protected override void HandleAttack(float distance)
    {
        if (health != null && health.IsDead)
            return;

        if (!attack.IsAttacking && distance > attackDistance)
        {
            ReleaseAttackSlot();
            ChangeState(State.Chase);
        }

        controller.Stop();

        if (!attack.IsAttacking)
            attack.TryAttack();
    }
}
