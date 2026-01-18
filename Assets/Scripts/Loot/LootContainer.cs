using System;
using System.Collections.Generic;
using UnityEngine;

public class LootContainer : MonoBehaviour, IInteractable
{
    public event Action OnLootEmptied;

    [Header("Loot")]
    [SerializeField] private List<LootEntry> loot = new();

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

        if (visualRoot != null)
            visualRoot.SetActive(true);
        // ❌ NO abrir loot aquí
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
        return loot;
    }

    public void RemoveLoot(LootEntry entry)
    {
        loot.Remove(entry);

        if (loot.Count == 0)
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
