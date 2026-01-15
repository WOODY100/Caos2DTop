using UnityEngine;

public class NPCWorldState : MonoBehaviour
{
    [Header("World Flags")]
    [SerializeField] private string requiredFlag;
    [SerializeField] private string completedFlag;

    public bool CanInteract()
    {
        if (!string.IsNullOrEmpty(requiredFlag))
            return WorldStateManager.Instance.HasFlag(requiredFlag);

        return true;
    }

    public void MarkCompleted()
    {
        if (!string.IsNullOrEmpty(completedFlag))
            WorldStateManager.Instance.SetFlag(completedFlag);
    }
}
