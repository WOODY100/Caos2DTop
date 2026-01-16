using UnityEngine;

public class NPCRepositionOnFlag : MonoBehaviour
{
    [Header("Reposition Condition")]
    [SerializeField] private string requiredFlag;

    [Header("Target Position")]
    [SerializeField] private Transform targetPosition;

    private bool moved;

    private void Start()
    {
        TryReposition();
    }

    private void Update()
    {
        if (!moved)
            TryReposition();
    }

    private void TryReposition()
    {
        if (moved)
            return;

        if (string.IsNullOrEmpty(requiredFlag))
            return;

        if (WorldStateManager.Instance.HasFlag(requiredFlag))
        {
            transform.position = targetPosition.position;
            moved = true;
        }
    }
}
