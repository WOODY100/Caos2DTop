using UnityEngine;
using TMPro;

public class InventoryHUD : MonoBehaviour
{
    [Header("Inventory Grid")]
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int slotCount = 25;

    [Header("Coins")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private ItemData coinItem;

    private InventorySlotUI[] slots;

    private void Awake()
    {
        slots = new InventorySlotUI[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, gridParent);
            slots[i] = slotGO.GetComponent<InventorySlotUI>();
        }
    }

    private void OnEnable()
    {
        InventoryManager.OnInventoryChanged += RefreshDelayed;
        Refresh();
        GamePauseManager.Instance.RequestPause(this);
    }

    private void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= RefreshDelayed;
        GamePauseManager.Instance.ReleasePause(this);
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        var inventory = InventoryManager.Instance.items;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Count)
                slots[i].SetItem(inventory[i]);
            else
                slots[i].Clear();
        }

        coinsText.text = InventoryManager.Instance.GetAmount(coinItem).ToString();
    }

    private void RefreshDelayed()
    {
        CancelInvoke(nameof(Refresh));
        Invoke(nameof(Refresh), 0f);
    }
}
