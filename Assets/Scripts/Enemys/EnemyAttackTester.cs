using UnityEngine;

public class EnemyAttackTester : MonoBehaviour
{
    private EnemyAttack attack;

    void Awake()
    {
        attack = GetComponent<EnemyAttack>();
        InvokeRepeating(nameof(TestAttack), 1f, 2f);
    }

    void TestAttack()
    {
        attack.TryAttack();
    }
}
