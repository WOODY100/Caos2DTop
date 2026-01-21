using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SaveData;

public class HotbarManager : MonoBehaviour, ISaveable
{
    private List<HotbarSlotSaveData> pendingHotbar;

    public static HotbarManager Instance { get; private set; }

    private HotbarSlotUI[] slots;
    private InventoryManager inventory;
    private PotionCooldownManager cooldown;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        inventory = InventoryManager.Instance;
        cooldown = FindFirstObjectByType<PotionCooldownManager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        slots = null; // 🔥 CLAVE
    }

    private bool EnsureSlots()
    {
        if (slots != null)
        {
            bool valid = true;
            foreach (var s in slots)
            {
                if (!s)
                {
                    valid = false;
                    break;
                }
            }

            if (valid && slots.Length > 0)
                return true;
        }

        slots = Object.FindObjectsByType<HotbarSlotUI>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        System.Array.Sort(slots, (a, b) => a.SlotIndex.CompareTo(b.SlotIndex));

        if (slots.Length > 0)
            TryApplyPendingHotbar();

        return slots.Length > 0;
    }

    public void AssignItemToSlot(int index, ItemData item)
    {
        if (!EnsureSlots())
            return;

        if (index < 0 || index >= slots.Length)
            return;

        int count = inventory.GetItemCount(item);
        slots[index].SetItem(item, count);
    }

    public void TryUseSlot(int index)
    {
        Debug.Log($"[HOTBAR] TryUseSlot({index})");

        if (!EnsureSlots())
        {
            Debug.Log("[HOTBAR] No hay slots");
            return;
        }

        if (index < 0 || index >= slots.Length)
        {
            Debug.Log("[HOTBAR] Index fuera de rango");
            return;
        }

        HotbarSlotUI slot = slots[index];

        if (slot.Item == null)
        {
            Debug.Log("[HOTBAR] Slot vacío");
            return;
        }

        Debug.Log("[HOTBAR] Item en slot: " + slot.Item.name);

        if (slot.Item is not IUsableItem usable)
        {
            Debug.Log("[HOTBAR] Item NO implementa IUsableItem");
            return;
        }

        bool used = usable.Use();
        Debug.Log("[HOTBAR] Use() devolvió: " + used);

        if (used)
        {
            inventory.RemoveItem(slot.Item, 1);
            RefreshSlot(index);
        }
    }

    private void RefreshSlot(int index)
    {
        if (slots == null || slots.Length == 0)
            return;

        if (index < 0 || index >= slots.Length)
            return;

        HotbarSlotUI slot = slots[index];
        if (slot == null)
            return;

        ItemData item = slot.Item;
        if (item == null)
            return;

        int count = inventory.GetItemCount(item);

        if (count <= 0)
            slot.Clear();
        else
            slot.SetItem(item, count);
    }
    
    public void SaveData(SaveData data)
    {
        if (!EnsureSlots())
            return;

        data.hotbarItems.Clear();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Item == null)
                continue;

            data.hotbarItems.Add(new HotbarSlotSaveData
            {
                slotIndex = i,
                itemID = slots[i].Item.itemID
            });
        }
    }

    public void LoadData(SaveData data)
    {
        pendingHotbar = data.hotbarItems;
    }

    private void TryApplyPendingHotbar()
    {
        if (pendingHotbar == null || pendingHotbar.Count == 0)
            return;

        foreach (var slot in slots)
            slot.SafeClear();

        foreach (var saved in pendingHotbar)
        {
            if (saved.slotIndex < 0 || saved.slotIndex >= slots.Length)
                continue;

            ItemData item = ItemDatabase.Instance.GetItem(saved.itemID);
            if (item == null)
                continue;

            int count = InventoryManager.Instance.GetItemCount(item);
            if (count <= 0)
                continue;

            slots[saved.slotIndex].SetItem(item, count);
        }

        pendingHotbar = null;
    }

    public void NotifySlotsUIReady()
    {
        if (!EnsureSlots())
            return;

        TryApplyPendingHotbar();
    }
}
