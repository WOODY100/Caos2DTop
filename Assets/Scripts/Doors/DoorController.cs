using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Requirements")]
    [SerializeField] private string requiredFlag;
    [SerializeField] private ItemData requiredKey;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D solidCollider;
    [SerializeField] private Collider2D triggerCollider;

    private bool isOpen;

    private void Start()
    {
        if (!string.IsNullOrEmpty(requiredFlag) &&
            WorldStateManager.Instance.HasFlag(requiredFlag))
        {
            OpenInstant();
        }
    }

    public void Interact()
    {
        if (isOpen)
            return;

        if (requiredKey != null &&
            !InventoryManager.Instance.HasItem(requiredKey))
        {
            // feedback: "Necesitas una llave"
            return;
        }

        Open();
    }

    private void Open()
    {
        isOpen = true;
        animator.SetBool("isOpen", true);

        solidCollider.enabled = false;
        triggerCollider.enabled = false;

        if (!string.IsNullOrEmpty(requiredFlag))
            WorldStateManager.Instance.SetFlag(requiredFlag);
    }

    private void OpenInstant()
    {
        isOpen = true;
        animator.Play("Open", 0, 1f);
        solidCollider.enabled = false;
        triggerCollider.enabled = false;
    }
}
