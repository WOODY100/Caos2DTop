using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [Header("Identity")]
    [SerializeField] private string chestID;

    [Header("Visuals")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openedSprite;

    [Header("Loot")]
    [SerializeField] private LootContainer lootContainer;

    [Header("Key Requirement")]
    [SerializeField] private bool requiresKey = false;
    [SerializeField] private ItemData requiredKey;
    [SerializeField] private bool consumeKeyOnOpen = true;

    //[Header("Open Conditions (Optional)")]
    //[SerializeField] private bool requiresWorldCondition = false;
    //[SerializeField] private string requiredWorldFlag;

    private bool isOpened;

    private void Awake()
    {
        if (WorldStateManager.Instance != null &&
        WorldStateManager.Instance.IsChestOpened(chestID))
        {
            SetOpenedFromSave();
        }
        else
        {
            ApplyVisual();
        }
    }

    public void Interact()
    {
        if (isOpened)
            return;

        // 🔒 Requiere llave
        if (requiresKey)
        {
            if (requiredKey == null)
            {
                Debug.LogWarning($"Chest {name} requiere llave pero no tiene ItemData asignado");
                return;
            }

            if (!InventoryManager.Instance.HasItem(requiredKey))
            {
                // ❌ No tiene la llave
                // Aquí luego puedes mostrar mensaje / sonido
                Debug.Log("Necesitas una llave para abrir este cofre");
                return;
            }

            // ✅ Tiene la llave
            if (consumeKeyOnOpen)
                InventoryManager.Instance.RemoveItem(requiredKey, 1);
        }

        // Abrir loot normalmente
        LootUI.Instance.Open(lootContainer);

        lootContainer.OnLootEmptied += OnLootEmptied;
    }

    private void OnLootEmptied()
    {
        OpenChest();
    }

    private void OpenChest()
    {
        isOpened = true;

        lootContainer.DisableLoot();
        ApplyVisual();

        // 🔹 Registrar en WorldState (luego)
        WorldStateManager.Instance?.MarkChestOpened(chestID);
    }

    private void ApplyVisual()
    {
        if (spriteRenderer == null)
            return;

        spriteRenderer.sprite = isOpened ? openedSprite : closedSprite;
    }

    // 🔹 Llamado desde WorldState al cargar
    public void SetOpenedFromSave()
    {
        isOpened = true;
        lootContainer.DisableLoot();
        ApplyVisual();
    }
}
