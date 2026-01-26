using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPoolManager : MonoBehaviour
{
    public static EnemyPoolManager Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> pools =
        new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public GameObject GetEnemy(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            pools[prefab] = pool;
        }

        GameObject enemy;

        if (pool.Count > 0)
        {
            enemy = pool.Dequeue();
            enemy.transform.SetPositionAndRotation(position, rotation);
            enemy.SetActive(true);
        }
        else
        {
            enemy = Instantiate(prefab, position, rotation);
        }

        enemy.GetComponent<IPoolable>()?.OnSpawned();

        return enemy;
    }

    public void ReleaseEnemy(GameObject enemy, GameObject prefab)
    {
        enemy.SetActive(false);

        if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            pools[prefab] = pool;
        }

        pool.Enqueue(enemy);
    }
}
