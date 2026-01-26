using UnityEngine;

public class Condition_StatMinimum : Condition
{
    public enum StatType
    {
        Health,
        Attack,
        Defense,
        Speed
    }

    [SerializeField] private StatType stat;
    [SerializeField] private int minimumValue;

    public override bool IsSatisfied()
    {
        PlayerStats stats = FindFirstObjectByType<PlayerStats>();
        if (stats == null)
            return false;

        return stat switch
        {
            StatType.Health => stats.maxHealth >= minimumValue,
            StatType.Attack => stats.attack >= minimumValue,
            StatType.Defense => stats.defense >= minimumValue,
            StatType.Speed => stats.speed >= minimumValue,
            _ => false
        };
    }
}
