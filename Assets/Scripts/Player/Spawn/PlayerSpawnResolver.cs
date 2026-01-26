using UnityEngine;
using System.Collections;

public class PlayerSpawnResolver : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;

        if (SpawnManager.Instance == null)
            yield break;

        string spawnID = SpawnManager.Instance.nextSpawnID;
        if (string.IsNullOrEmpty(spawnID))
            yield break;

        // 🔹 LIMPIAR SPAWN ANTES DE USARLO
        SpawnManager.Instance.nextSpawnID = null;

        Vector3 oldPosition = transform.position;

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

        if (CameraTransitionController.Instance != null)
        {
            CameraTransitionController.Instance.NotifyTeleport(
                oldPosition,
                transform.position
            );
        }

        if (FadeManager.Instance != null)
            yield return FadeManager.Instance.FadeIn();

        PlayerController pc2 = GetComponent<PlayerController>();
        pc2?.SetInputEnabled(true);
    }
}
