using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class InventoryHUD : MonoBehaviour
{
    //public static GameObject InventoryHud;

    //public InventoryHUD Instance { get; private set; }
    //public static object inventoryHud { get; internal set; }

    [Header("Inventory Grid")]
    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int slotCount = 25;

    [Header("Coins")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private ItemData coinItem;

    private InventorySlotUI[] slots;

    private bool isOpen;

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

        if (GamePauseManager.Instance != null)
            GamePauseManager.Instance.ReleasePause(this);
    }

    public void Open()
    {
        if (isOpen) return;

        // 🔒 No abrir inventario durante LevelUp
        if (GamePauseManager.Instance.IsPausedBy<LevelUpUI>())
            return;

        isOpen = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        if (!isOpen) return;

        ItemTooltipUI.Instance?.Hide(); // 🔥 BLINDAJE

        isOpen = false;
        gameObject.SetActive(false);
    }


    public void Toggle()
    {
        // 🚫 Si intento abrir pero el juego está pausado por otra UI (PauseMenu, LevelUp)
        if (!isOpen && GamePauseManager.Instance.IsPaused())
            return;

        if (isOpen)
            Close();
        else
            Open();
    }

    public void Refresh()
    {
        Debug.Log($"[HUD] Items en inventario: {InventoryManager.Instance.items.Count}");

        var inventory = InventoryManager.Instance.items;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            if (i < inventory.Count)
                slots[i].SetItem(inventory[i]);
            else
                slots[i].Clear();
        }

        coinsText.text = InventoryManager.Instance.GetCoins().ToString();
    }

    private void RefreshDelayed()
    {
        CancelInvoke(nameof(Refresh));
        Invoke(nameof(Refresh), 0f);
    }
}
