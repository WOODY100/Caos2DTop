using UnityEngine;
using System.Collections;

public class PlayerSpawnResolver : MonoBehaviour
{
    private IEnumerator Start()
    {
        // 1️⃣ Esperar 1 frame para que Cinemachine y la escena estén listos
        yield return null;

        if (SpawnManager.Instance == null)
            yield break;

        string spawnID = SpawnManager.Instance.nextSpawnID;
        if (string.IsNullOrEmpty(spawnID))
            yield break;

        // 2️⃣ Guardar posición previa (importante para Cinemachine)
        Vector3 oldPosition = transform.position;

        // 3️⃣ Buscar el SpawnPoint correcto
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

        // 4️⃣ Notificar a Cinemachine que fue un TELEPORT
        if (CameraTransitionController.Instance != null)
        {
            CameraTransitionController.Instance.NotifyTeleport(
                oldPosition,
                transform.position
            );
        }

        // 5️⃣ Fade In DESPUÉS de que TODO está en su lugar
        if (FadeManager.Instance != null)
            yield return FadeManager.Instance.FadeIn();

        // 6️⃣ Reactivar input del jugador
        PlayerController pc = GetComponent<PlayerController>();
        pc?.SetInputEnabled(true);
    }
}
