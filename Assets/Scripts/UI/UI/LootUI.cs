using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootUI : MonoBehaviour
{
    [SerializeField] private ScrollRect parentScrollRect;
    public static LootUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform content;
    [SerializeField] private LootItemUI itemPrefab;

    private LootContainer currentContainer;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        panel.SetActive(false);
    }

    public void Open(LootContainer container)
    {
        if (container == null) return;

        currentContainer = container;
        panel.SetActive(true);
        Refresh();
    }

    public void Refresh()
    {
        if (currentContainer == null) return;

        foreach (Transform child in content)
            Destroy(child.gameObject);

        List<LootEntry> loot = currentContainer.GetLoot();

        foreach (var entry in loot)
        {
            LootItemUI ui = Instantiate(itemPrefab, content);
            ui.Setup(entry, currentContainer);
        }
    }

    public void Close()
    {
        if (!panel.activeSelf)
            return;

        panel.SetActive(false);
        currentContainer = null;
    }

    public void CloseIfCurrent(LootContainer container)
    {
        if (currentContainer == container)
            Close();
    }

    public void OnCancel()
    {
        if (!panel.activeSelf)
            return;

        Close();
    }
}
