using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootItemUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text amountText;

    private LootEntry entry;
    private LootContainer container;

    public void Setup(LootEntry entry, LootContainer container)
    {
        this.entry = entry;
        this.container = container;

        // ICONO
        if (icon != null)
        {
            icon.sprite = entry.item.icon;
            icon.enabled = true; // 🔥 CLAVE
            icon.color = Color.white; // seguridad
        }

        // TEXTO
        if (nameText != null)
            nameText.text = entry.item.itemName;

        if (amountText != null)
            amountText.text = entry.amount > 1 ? $"x{entry.amount}" : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (entry == null || container == null)
            return;

        if (InventoryManager.Instance == null)
        {
            Debug.LogError("InventoryManager.Instance es null");
            return;
        }

        bool added = InventoryManager.Instance.AddItem(entry.item, entry.amount);
        if (!added) return;

        container.RemoveLoot(entry);
        Destroy(gameObject);

        LootUI.Instance?.Refresh();
    }
}
