using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(Collider2D))]
public class EnemySpawnZone : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private EnemySpawnData spawnData;

    [Header("Rules")]
    [SerializeField] private float minDistanceFromPlayer = 2.5f;
    [SerializeField] private int maxSpawnAttempts = 15;

    private Collider2D zoneCollider;
    private readonly List<GameObject> activeEnemies = new();

    private bool playerInside;
    private bool respawning;
    private Transform player;

    void Awake()
    {
        zoneCollider = GetComponent<Collider2D>();
        zoneCollider.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;
        player = other.transform;

        if (activeEnemies.Count == 0)
            SpawnWave();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;

        if (activeEnemies.Count == 0 && !respawning)
            StartCoroutine(RespawnRoutine());
    }

    // =========================
    // SPAWN
    // =========================
    private void SpawnWave()
    {
        int playerLevel =
            FindFirstObjectByType<PlayerExperience>()?.level ?? 1;

        // 👑 Boss (opcional)
        GameObject boss = BossSpawner.TrySpawnBoss(
            spawnData,
            playerLevel,
            GetRandomPointInZone,
            IsValidSpawnPosition
        );

        if (boss != null)
            RegisterEnemy(boss);

        // 👾 Enemigos normales
        List<EnemySpawnEntry> validEnemies = spawnData.enemies.FindAll(e =>
            !e.isBoss &&
            playerLevel >= e.minPlayerLevel &&
            playerLevel <= e.maxPlayerLevel
        );

        if (validEnemies.Count == 0)
            return;

        int count = Random.Range(spawnData.minEnemies, spawnData.maxEnemies + 1);
        if (boss != null)
            count = Mathf.Max(0, count - 1);

        for (int i = 0; i < count; i++)
            TrySpawnEnemy(validEnemies);
    }

    private void TrySpawnEnemy(List<EnemySpawnEntry> pool)
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector2 pos = GetRandomPointInZone();
            if (!IsValidSpawnPosition(pos))
                continue;

            EnemySpawnEntry entry = PickWeighted(pool);
            GameObject enemy = Instantiate(entry.enemyPrefab, pos, Quaternion.identity);
            RegisterEnemy(enemy);
            return;
        }
    }

    private void RegisterEnemy(GameObject enemy)
    {
        activeEnemies.Add(enemy);

        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
            health.OnEnemyDied += () => OnEnemyDied(enemy);
    }

    private void OnEnemyDied(GameObject enemy)
    {
        activeEnemies.Remove(enemy);

        if (!playerInside && activeEnemies.Count == 0 && !respawning)
            StartCoroutine(RespawnRoutine());
    }

    // =========================
    // RESPAWN
    // =========================
    private IEnumerator RespawnRoutine()
    {
        respawning = true;
        yield return new WaitForSeconds(spawnData.respawnDelay);

        if (!playerInside)
            SpawnWave();

        respawning = false;
    }

    // =========================
    // HELPERS
    // =========================
    private Vector2 GetRandomPointInZone()
    {
        Bounds b = zoneCollider.bounds;
        return new Vector2(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y)
        );
    }

    private bool IsValidSpawnPosition(Vector2 pos)
    {
        if (player != null &&
            Vector2.Distance(pos, player.position) < minDistanceFromPlayer)
            return false;

        return NavMesh.SamplePosition(pos, out _, 0.5f, NavMesh.AllAreas);
    }

    private EnemySpawnEntry PickWeighted(List<EnemySpawnEntry> list)
    {
        float total = 0f;
        foreach (var e in list) total += e.weight;

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
