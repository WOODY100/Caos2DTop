using UnityEngine;

public class Condition_WorldFlag : Condition
{
    [SerializeField] private string requiredFlag;

    public override bool IsSatisfied()
    {
        if (string.IsNullOrEmpty(requiredFlag))
            return true;

        return WorldStateManager.Instance.HasFlag(requiredFlag);
    }
}
