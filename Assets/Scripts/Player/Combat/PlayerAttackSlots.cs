using UnityEngine;
using System.Collections.Generic;

public class PlayerAttackSlots : MonoBehaviour
{
    public int maxAttackers = 1;

    private HashSet<EnemyAIBase> attackers = new();

    public bool RequestSlot(EnemyAIBase enemy)
    {
        if (attackers.Contains(enemy))
            return true;

        if (attackers.Count >= maxAttackers)
            return false;

        attackers.Add(enemy);
        return true;
    }

    public void ReleaseSlot(EnemyAIBase enemy)
    {
        if (attackers.Contains(enemy))
            attackers.Remove(enemy);
    }

    public int CurrentAttackers => attackers.Count;
}
