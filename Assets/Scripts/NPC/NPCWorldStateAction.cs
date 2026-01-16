using UnityEngine;

public class NPCWorldStateAction : MonoBehaviour
{
    [Header("Optional Flag")]
    [SerializeField] private string flagOnDialogueEnd;
    private bool executed;

    public void Execute()
    {
        if (executed)
            return;

        if (!string.IsNullOrEmpty(flagOnDialogueEnd))
        {
            WorldStateManager.Instance.SetFlag(flagOnDialogueEnd);
            executed = true;
        }
    }
}
