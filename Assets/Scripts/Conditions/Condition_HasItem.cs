using UnityEngine;

public class Condition_HasItem : Condition
{
    [SerializeField] private ItemData requiredItem;

    public override bool IsSatisfied()
    {
        if (requiredItem == null)
            return true;

        return InventoryManager.Instance.HasItem(requiredItem);
    }
}
