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
        if (distance > attackDistance)
        {
            ReleaseAttackSlot();
            ChangeState(State.Chase);
            return;
        }

        attack.TryAttack();
    }
}
