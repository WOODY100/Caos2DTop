using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LootContainer))]
public class EnemyLootDrop : MonoBehaviour
{
    [Header("Interaction")]
    public InputActionReference interactAction;
    public bool autoOpen = false;

    private LootContainer lootContainer;
    private bool canLoot;

    void Awake()
    {
        lootContainer = GetComponent<LootContainer>();
    }

    void OnEnable()
    {
        if (interactAction != null)
            interactAction.action.performed += OnInteract;
    }

    void OnDisable()
    {
        if (interactAction != null)
            interactAction.action.performed -= OnInteract;
    }

    // 🔔 Llamado por EnemyHealth al morir
    public void EnableLoot()
    {
        canLoot = true;

        if (autoOpen)
        {
            OpenLoot();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!canLoot) return;
        if (!other.CompareTag("Player")) return;

        interactAction?.action.Enable();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        interactAction?.action.Disable();
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!canLoot) return;
        OpenLoot();
    }

    void OpenLoot()
    {
        LootUI.Instance.Open(lootContainer);
    }
}
