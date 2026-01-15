using UnityEngine;

public class NPCActionExecutor : MonoBehaviour
{
    public void GiveItem(ItemData item, int amount)
    {
        InventoryManager.Instance.AddItem(item, amount);
    }

    public void SetFlag(string flagID)
    {
        WorldStateManager.Instance.SetFlag(flagID);
    }
}

