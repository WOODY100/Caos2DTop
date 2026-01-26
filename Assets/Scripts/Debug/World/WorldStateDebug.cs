using UnityEngine;

public class WorldStateDebug : MonoBehaviour
{
    [ContextMenu("Log World Flags")]
    private void LogFlags()
    {
        Debug.Log("Flags actuales:");
        foreach (var flag in WorldStateManager.Instance.GetAllFlags())
        {
            Debug.Log(flag);
        }
    }
}
