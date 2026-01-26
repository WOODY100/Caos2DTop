using System;
using System.Collections.Generic;
using UnityEngine;

public class LootContainer : MonoBehaviour, IInteractable
{
    public event Action OnLootEmptied;

    [Header("Loot")]
    [SerializeField] private List<LootEntry> lootTable = new();
    private List<LootEntry> generatedLoot = new();

    [Header("State")]
    public bool IsOpened { get; private set; }

    [Header("Components")]
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private GameObject visualRoot;

    private bool playerInside;

    void Awake()
    {
        if (triggerCollider == null)
            triggerCollider = GetComponent<Collider2D>();

        DisableLoot();
    }

    public void EnableLoot()
    {
        IsOpened = false;
        triggerCollider.enabled = true;

        GenerateLoot(); // 🔴 AQUÍ

        if (visualRoot != null)
            visualRoot.SetActive(true);
    }

    public void DisableLoot()
    {
        triggerCollider.enabled = false;

        if (visualRoot != null)
            visualRoot.SetActive(false);
    }

    public void Interact()
    {
        if (IsOpened)
            return;

        if (!playerInside)
            return; // 🔹 seguridad extra

        if (LootUI.Instance == null)
        {
            Debug.LogWarning("LootUI no existe en la escena");
            return;
        }

        LootUI.Instance.Open(this);
    }

    public List<LootEntry> GetLoot()
    {
        return generatedLoot;
    }

    public void RemoveLoot(LootEntry entry)
    {
        generatedLoot.Remove(entry);

        if (generatedLoot.Count == 0)
        {
            IsOpened = true;
            OnLootEmpty();
            OnLootEmptied?.Invoke();
        }
    }

    protected virtual void OnLootEmpty()
    {
        LootUI.Instance?.Close();
        Destroy(gameObject);
    }

    private void GenerateLoot()
    {
        generatedLoot.Clear();

        foreach (var entry in lootTable)
        {
            if (entry.item == null)
                continue;

            bool shouldDrop = entry.guaranteed;

            if (!shouldDrop)
            {
                float roll = UnityEngine.Random.Range(0f, 100f);
                shouldDrop = roll <= entry.dropChance;
            }

            if (!shouldDrop)
                continue;

            LootEntry generated = new LootEntry
            {
                item = entry.item,
                dropChance = entry.dropChance,
                guaranteed = entry.guaranteed,
                minAmount = entry.minAmount,
                maxAmount = entry.maxAmount,
                amount = UnityEngine.Random.Range(entry.minAmount, entry.maxAmount + 1)
            };

            generatedLoot.Add(generated);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;
        LootUI.Instance?.CloseIfCurrent(this);
    }
}
