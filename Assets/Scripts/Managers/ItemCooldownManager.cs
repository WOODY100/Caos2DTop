using System.Collections.Generic;
using UnityEngine;

public class ItemCooldownManager : MonoBehaviour
{
    public static ItemCooldownManager Instance { get; private set; }

    private Dictionary<string, float> cooldownEndTimes =
        new Dictionary<string, float>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public bool IsOnCooldown(ItemData item)
    {
        if (item == null) return false;

        if (!cooldownEndTimes.TryGetValue(item.itemID, out float endTime))
            return false;

        return Time.time < endTime;
    }

    public float GetRemaining(ItemData item)
    {
        if (item == null) return 0f;

        if (!cooldownEndTimes.TryGetValue(item.itemID, out float endTime))
            return 0f;

        return Mathf.Max(0f, endTime - Time.time);
    }

    public float GetTotal(ItemData item)
    {
        return item is IUsableItem usable ? usable.CooldownDuration : 0f;
    }

    public void StartCooldown(ItemData item)
    {
        if (item is not IUsableItem usable)
            return;

        cooldownEndTimes[item.itemID] =
            Time.time + usable.CooldownDuration;
    }
}
