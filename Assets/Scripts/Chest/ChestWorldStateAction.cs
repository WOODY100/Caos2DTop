using UnityEngine;

public class ChestWorldStateAction : MonoBehaviour
{
    [Header("Optional WorldState Action")]
    [SerializeField] private string flagOnOpen;
    private bool executed;

    public void Execute()
    {
        if (executed)
            return;

        if (!string.IsNullOrEmpty(flagOnOpen))
        {
            WorldStateManager.Instance.SetFlag(flagOnOpen);
            executed = true;
        }
    }
}
