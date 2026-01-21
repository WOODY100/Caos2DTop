using UnityEngine;

public static class PlayerStatsProvider
{
    private static PlayerStats cachedStats;

    public static PlayerStats Get()
    {
        if (cachedStats == null)
        {
            cachedStats = Object.FindFirstObjectByType<PlayerStats>();
        }

        return cachedStats;
    }
}
