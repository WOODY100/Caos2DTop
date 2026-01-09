using System.Collections.Generic;
using UnityEngine;

public class LootContainer : MonoBehaviour
{
    [SerializeField] private List<LootEntry> loot = new();
    public bool IsOpened { get; private set; }

    public List<LootEntry> GetLoot()
    {
        return loot;
    }

    public void RemoveLoot(LootEntry entry)
    {
        loot.Remove(entry);

        if (loot.Count == 0)
            IsOpened = true;
    }
}
