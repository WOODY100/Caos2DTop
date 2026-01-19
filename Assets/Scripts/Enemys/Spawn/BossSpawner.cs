using UnityEngine;
using System.Collections.Generic;

public class BossSpawner
{
    public static GameObject TrySpawnBoss(
        EnemySpawnData data,
        int playerLevel,
        System.Func<Vector2> getSpawnPosition,
        System.Func<Vector2, bool> isValidPosition)
    {
        if (data.bossSpawnChance <= 0f)
            return null;

        if (Random.value * 100f > data.bossSpawnChance)
            return null;

        List<EnemySpawnEntry> validBosses = data.enemies.FindAll(e =>
            e.isBoss &&
            playerLevel >= e.minPlayerLevel &&
            playerLevel <= e.maxPlayerLevel
        );

        if (validBosses.Count == 0)
            return null;

        EnemySpawnEntry chosen = PickWeighted(validBosses);

        for (int i = 0; i < 10; i++)
        {
            Vector2 pos = getSpawnPosition();
            if (!isValidPosition(pos))
                continue;

            return Object.Instantiate(chosen.enemyPrefab, pos, Quaternion.identity);
        }

        return null;
    }

    private static EnemySpawnEntry PickWeighted(List<EnemySpawnEntry> list)
    {
        float total = 0f;
        foreach (var e in list)
            total += e.weight;

        float roll = Random.value * total;
        float current = 0f;

        foreach (var e in list)
        {
            current += e.weight;
            if (roll <= current)
                return e;
        }

        return list[0];
    }
}
