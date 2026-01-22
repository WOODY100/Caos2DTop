using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
    }

    private void OnDisable()
    {
        InventoryManager.OnInventoryChanged -= RefreshDelayed;
    }

    public void Open()
    {
        if (isOpen) return;

        if (UIModalManager.Instance != null &&
            !UIModalManager.Instance.RequestOpen(this))
            return;

        GameStateManager.Instance.SetState(GameState.Inventory);

        isOpen = true;
        gameObject.SetActive(true);

        EquipmentManager.Instance?.RefreshUIFromEquipped();

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
                controller.SetInputEnabled(true);
                controller.ForceCancelMovement();
        }

        // 🔑 Resetear estado de interacción del equipo
        EquipmentManager.Instance?.ResetInteractionState();

    }

    public void Close()
    {
        if (!isOpen) return;

        ItemTooltipUI.Instance?.Hide();

        isOpen = false;
        gameObject.SetActive(false);

        //EquipmentManager.Instance?.ClearUISlots();

        if (UIModalManager.Instance != null)
            UIModalManager.Instance.Close(this);

        GameStateManager.Instance.SetState(GameState.Playing);

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
                controller.SetInputEnabled(true);
        }
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }


    public void Refresh()
    {
        var inventory = InventoryManager.Instance.items;

        // 🔹 Agrupar items por ID
        Dictionary<ItemData, int> grouped = new Dictionary<ItemData, int>();

        foreach (var item in inventory)
        {
            if (item == null) continue;

            if (grouped.ContainsKey(item))
                grouped[item]++;
            else
                grouped[item] = 1;
        }

        int index = 0;

        foreach (var pair in grouped)
        {
            if (index >= slots.Length)
                break;

            slots[index].SetItem(pair.Key, pair.Value);
            index++;
        }

        // Limpiar slots restantes
        for (int i = index; i < slots.Length; i++)
        {
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
