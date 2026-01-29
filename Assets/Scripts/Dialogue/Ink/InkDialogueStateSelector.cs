using System.Collections.Generic;
using UnityEngine;

public static class InkDialogueStateSelector
{
    private static Dictionary<string, NPCDialogueConfig> cache;

    private static void BuildCache()
    {
        if (cache != null)
            return;

        cache = new Dictionary<string, NPCDialogueConfig>();
        var configs = Resources.LoadAll<NPCDialogueConfig>("Dialogue");

        foreach (var config in configs)
        {
            if (!string.IsNullOrEmpty(config.npcID))
                cache[config.npcID] = config;
        }
    }

    public static string GetStartKnot(string npcID)
    {
        BuildCache();

        if (!cache.TryGetValue(npcID, out var config))
        {
            Debug.LogError($"[Ink] No DialogueConfig for NPC '{npcID}'");
            return null;
        }

        var world = WorldStateManager.Instance;

        foreach (var rule in config.startRules)
        {
            if (!world.HasFlag(rule.requiredFlag))
                return rule.startKnot;
        }

        return config.defaultKnot;
    }
}
