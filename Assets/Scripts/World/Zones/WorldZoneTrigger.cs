using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class WorldZoneTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private bool requirePlayer = true;

    [Header("World Flags")]
    [SerializeField] private string flagOnEnter;
    [SerializeField] private string flagOnExit;

    [Header("Unity Events (Optional)")]
    public UnityEvent onEnter;
    public UnityEvent onExit;

    private bool hasTriggered;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (requirePlayer && !other.CompareTag("Player"))
            return;

        if (triggerOnce && hasTriggered)
            return;

        hasTriggered = true;

        if (!string.IsNullOrEmpty(flagOnEnter))
            WorldStateManager.Instance?.SetFlag(flagOnEnter);

        onEnter?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (requirePlayer && !other.CompareTag("Player"))
            return;

        if (!string.IsNullOrEmpty(flagOnExit))
            WorldStateManager.Instance?.SetFlag(flagOnExit);

        onExit?.Invoke();
    }
}
