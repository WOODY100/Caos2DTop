using UnityEngine;

public class PlayerSpawnResolver : MonoBehaviour
{
    private void Start()
    {
        if (SpawnManager.Instance == null) return;

        string spawnID = SpawnManager.Instance.nextSpawnID;
        if (string.IsNullOrEmpty(spawnID)) return;

        SpawnPoint[] points = Object.FindObjectsByType<SpawnPoint>(
            FindObjectsSortMode.None
        );

        foreach (var sp in points)
        {
            if (sp.spawnID == spawnID)
            {
                transform.position = sp.transform.position;
                break;
            }
        }
    }
}
