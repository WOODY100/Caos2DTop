using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [HideInInspector] public string nextSpawnID;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetSpawn(string spawnID)
    {
        nextSpawnID = spawnID;
    }
}
