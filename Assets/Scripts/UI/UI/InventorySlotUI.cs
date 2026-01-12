using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler

{
    [SerializeField] private Image icon;
    [SerializeField] private Image arrowUp;
    [SerializeField] private Image arrowDown;

    private ItemData item;

    public void SetItem(ItemData newItem)
    {
        item = newItem;

        // 🔥 ACTIVAR SLOT
        gameObject.SetActive(true);

        if (item == null)
        {
            Clear();
            return;
        }

        if (icon != null)
        {
            icon.sprite = item.icon;
            icon.color = Color.white;
            icon.enabled = true;
        }

        UpdateComparison();
    }


    private void UpdateComparison()
    {
        if (arrowUp != null) arrowUp.enabled = false;
        if (arrowDown != null) arrowDown.enabled = false;

        if (!EquipmentManager.Instance || item == null)
            return;

        ItemData equipped = EquipmentManager.Instance.GetEquipped(item.type);
        if (equipped == null)
            return;

        int newScore = item.GetStatScore();
        int equippedScore = equipped.GetStatScore();

        if (newScore > equippedScore && arrowUp != null)
            arrowUp.enabled = true;
        else if (newScore < equippedScore && arrowDown != null)
            arrowDown.enabled = true;
    }

    public void Clear()
    {
        item = null;

        ItemTooltipUI.Instance?.Hide(); // 🔥 BLINDAJE

        if (icon != null)
        {
            icon.sprite = null;
            icon.enabled = false;
        }

        if (arrowUp != null) arrowUp.enabled = false;
        if (arrowDown != null) arrowDown.enabled = false;

        gameObject.SetActive(false);
    }




    public void OnPointerClick(PointerEventData eventData)
    {
        if (item == null) return;

        bool equipped = EquipmentManager.Instance.Equip(item);

        if (equipped)
        {
            // 🔥 CERRAR TOOLTIP EXPLÍCITAMENTE
            ItemTooltipUI.Instance?.Hide();

            InventoryManager.Instance.RemoveItem(item);
            Clear();
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null) return;
        ItemTooltipUI.Instance.Show(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltipUI.Instance.Hide();
    }
}
