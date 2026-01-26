using UnityEngine;

public class BossResetZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAIBase enemyAI = other.GetComponent<EnemyAIBase>();
        if (enemyAI == null) return;

        enemyAI.ForceHardReset();
    }
}