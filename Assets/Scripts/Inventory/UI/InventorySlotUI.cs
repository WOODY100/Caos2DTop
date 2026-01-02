using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image icon;

    private ItemData item;

    public void SetItem(ItemData newItem)
    {
        item = newItem;

        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
        }
        else
        {
            Clear();
        }
    }

    public void Clear()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        bool equipped = EquipmentManager.Instance.Equip(item);

        if (equipped)
        {
            InventoryManager.Instance.RemoveItem(item);
            Clear();
        }
    }

}
