using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Spawning/Enemy Spawn Data")]
public class EnemySpawnData : ScriptableObject
{
    [Header("Enemy Pool")]
    public List<EnemySpawnEntry> enemies = new();

    [Header("Spawn Count")]
    public int minEnemies = 2;
    public int maxEnemies = 5;

    [Header("Respawn")]
    public float respawnDelay = 10f;

    [Header("Boss Settings")]
    [Range(0f, 100f)]
    public float bossSpawnChance = 5f; // 0 = nunca
}
