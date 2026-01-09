using System.Collections.Generic;
using UnityEngine;

public class LootUI : MonoBehaviour
{
    public static LootUI Instance;

    [Header("UI")]
    public GameObject panel;
    public Transform content;
    public LootItemUI itemPrefab;

    private LootContainer currentContainer;
    private ChestInteractable currentChest;


    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Open(LootContainer container)
    {
        currentContainer = container;
        currentChest = container.GetComponent<ChestInteractable>();

        panel.SetActive(true);
        Refresh();
    }

    public void Refresh()
    {
        if (itemPrefab == null)
        {
            Debug.LogError("LootUI → itemPrefab NO asignado");
            return;
        }

        foreach (Transform child in content)
            Destroy(child.gameObject);

        if (currentContainer == null) return;

        List<LootEntry> loot = currentContainer.GetLoot();

        foreach (var entry in loot)
        {
            LootItemUI ui = Instantiate(itemPrefab, content);
            ui.Setup(entry, currentContainer);
        }

        // Si no queda loot → cerrar
        if (loot.Count == 0)
        {
            currentChest?.OnLootEmpty();
            Close();
        }
    }

    public void Close()
    {
        panel.SetActive(false);
        currentContainer = null;
    }
}
