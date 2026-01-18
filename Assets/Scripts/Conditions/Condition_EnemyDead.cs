using UnityEngine;

public class Condition_EnemyDead : Condition
{
    [SerializeField] private string enemyID;

    public override bool IsSatisfied()
    {
        if (string.IsNullOrEmpty(enemyID))
            return true;

        return WorldStateManager.Instance.IsEnemyDead(enemyID);
    }
}
