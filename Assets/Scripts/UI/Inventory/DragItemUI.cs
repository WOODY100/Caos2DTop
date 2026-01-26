using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DragItemUI : MonoBehaviour
{
    public static DragItemUI Instance;

    [SerializeField] private Image icon;

    public ItemData Item { get; private set; }
    public InventorySlotUI SourceSlot { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Clear();
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;
        if (Mouse.current == null) return;

        transform.position = Mouse.current.position.ReadValue();
    }

    // ─────────────────────────────
    // DRAG CONTROL
    // ─────────────────────────────
    public void BeginDrag(ItemData item, InventorySlotUI source)
    {
        Item = item;
        SourceSlot = source;

        icon.sprite = item.icon;
        icon.color = Color.white;
        icon.raycastTarget = false;

        gameObject.SetActive(true);
    }

    public void UpdatePosition(Vector2 screenPos)
    {
        transform.position = screenPos;
    }

    public void EndDrag()
    {
        Clear();
    }

    // ─────────────────────────────
    // CLEANUP
    // ─────────────────────────────
    public void Clear()
    {
        Item = null;
        SourceSlot = null;

        icon.sprite = null;
        icon.color = new Color(1, 1, 1, 0);

        gameObject.SetActive(false);
    }
}
