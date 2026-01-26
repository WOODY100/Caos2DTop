using UnityEngine;

public class Condition_PlayerLevel : Condition
{
    [SerializeField] private int minimumLevel = 1;

    public override bool IsSatisfied()
    {
        PlayerExperience exp = FindFirstObjectByType<PlayerExperience>();
        if (exp == null)
            return false;

        return exp.level >= minimumLevel;
    }
}
