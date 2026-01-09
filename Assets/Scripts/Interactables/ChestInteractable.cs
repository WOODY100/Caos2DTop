using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LootContainer))]
[RequireComponent(typeof(SpriteRenderer))]
public class ChestInteractable : MonoBehaviour
{
    [Header("Interaction")]
    public InputActionReference interactAction;

    [Header("Sprites")]
    public Sprite closedSprite;
    public Sprite openSprite;

    private LootContainer lootContainer;
    private SpriteRenderer sr;

    private bool playerInside;

    void Awake()
    {
        lootContainer = GetComponent<LootContainer>();
        sr = GetComponent<SpriteRenderer>();

        if (closedSprite != null)
            sr.sprite = closedSprite;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        interactAction?.action.Enable();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        interactAction?.action.Disable();
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!playerInside) return;
        if (lootContainer.IsOpened) return;

        OpenChest();
    }

    void OpenChest()
    {
        LootUI.Instance.Open(lootContainer);
    }

    // 🔔 Llamado cuando el loot queda vacío
    public void OnLootEmpty()
    {
        if (openSprite != null)
            sr.sprite = openSprite;
    }
}
