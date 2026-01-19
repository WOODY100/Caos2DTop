using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("Slot Config")]
    public ItemType acceptedType;
    public Image icon;

    private ItemData equippedItem;

    private void Awake()
    {
        EquipmentManager.Instance?.RegisterSlot(this);
        if (icon != null)
            icon.enabled = false;
    }

    private void OnEnable()
    {
        RefreshFromEquipment();
    }

    public bool CanEquip(ItemData item)
    {
        return item != null && item.type == acceptedType;
    }

    public ItemData Equip(ItemData item)
    {
        if (!CanEquip(item)) return null;

        ItemData previousItem = equippedItem;

        equippedItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;

        return previousItem;
    }

    public ItemData Unequip()
    {
        if (equippedItem == null) return null;

        ItemData oldItem = equippedItem;
        equippedItem = null;

        icon.sprite = null;
        icon.enabled = false;

        return oldItem;
    }

    public ItemData GetItem() => equippedItem;

    public void Clear()
    {
        equippedItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (equippedItem == null) return;

        EquipmentManager.Instance.Unequip(acceptedType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equippedItem == null) return;

        ItemTooltipUI.Instance.Show(equippedItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipUI.Instance.Hide();
    }

    public void RefreshFromEquipment()
    {
        if (EquipmentManager.Instance == null)
            return;

        ItemData item = EquipmentManager.Instance.GetEquipped(acceptedType);

        if (item != null)
        {
            equippedItem = item;
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        else
        {
            equippedItem = null;
            icon.sprite = null;
            icon.enabled = false;
        }
    }
}
