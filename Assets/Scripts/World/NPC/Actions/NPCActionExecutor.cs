using UnityEngine;

public class NPCActionExecutor : MonoBehaviour
{
    public void GiveItem(ItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return;

        InventoryManager.Instance.AddItem(item, amount);
    }

    public void GiveItemOnce(ItemData item, int amount, string flagID)
    {
        if (item == null || amount <= 0)
            return;

        if (!string.IsNullOrEmpty(flagID) &&
            WorldStateManager.Instance.HasFlag(flagID))
            return;

        InventoryManager.Instance.AddItem(item, amount);

        if (!string.IsNullOrEmpty(flagID))
            WorldStateManager.Instance.SetFlag(flagID);
    }

    public void SetFlag(string flagID)
    {
        if (string.IsNullOrEmpty(flagID))
            return;

        WorldStateManager.Instance.SetFlag(flagID);
    }
}
