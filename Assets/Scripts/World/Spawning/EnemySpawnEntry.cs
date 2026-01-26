using UnityEngine;

[System.Serializable]
public class EnemySpawnEntry
{
    public GameObject enemyPrefab;

    [Range(0f, 100f)]
    public float weight = 10f;

    [Header("Player Level Range")]
    public int minPlayerLevel = 1;
    public int maxPlayerLevel = 999;

    [Header("Boss")]
    public bool isBoss = false;
}
