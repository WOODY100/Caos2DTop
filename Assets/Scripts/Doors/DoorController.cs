using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [Header("Requirements")]
    [SerializeField] private string requiredFlag;
    [SerializeField] private ItemData requiredKey;
    
    [Header("Key Options")]
    [SerializeField] private bool consumeKeyOnOpen = false;

    [Header("Completion")]
    [SerializeField] private string flagOnExit; // 👈 NUEVO

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
            FeedbackPopupUI.Instance?.Show( "Necesitas una llave", transform.position);
            return;
        }

        Open();
    }

    private void Open()
    {
        isOpen = true;
        animator.SetBool("isOpen", true);

        DisableColliders();

        // 🔑 Consumir la llave si corresponde
        if (consumeKeyOnOpen && requiredKey != null)
        {
            InventoryManager.Instance.RemoveItem(requiredKey, 1);
        }
    }

    private void OpenInstant()
    {
        isOpen = true;

        // 🔒 Fuerza el estado abierto SIN reproducir animación
        animator.SetBool("isOpen", true);
        animator.Update(0f); // aplica el estado inmediatamente

        DisableColliders();
    }

    private void DisableColliders()
    {
        if (solidCollider != null)
            solidCollider.enabled = false;

        if (triggerCollider != null)
            triggerCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isOpen)
            return;

        if (!other.CompareTag("Player"))
            return;

        if (!string.IsNullOrEmpty(flagOnExit))
        {
            WorldStateManager.Instance.SetFlag(flagOnExit);
        }
    }
}
