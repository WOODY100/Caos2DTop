using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BossIntroCameraTrigger : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private CinemachineCamera bossCamera;

    [Header("Timing")]
    [SerializeField] private float focusTime = 1.0f;
    [SerializeField] private float pauseTime = 0.5f;
    [SerializeField] private float returnTime = 0.9f;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(CameraSequence());

        GetComponent<Collider2D>().enabled = false;
    }

    private IEnumerator CameraSequence()
    {
        GameStateManager.Instance?.SetState(GameState.Transition);

        // 🎥 Enfocar boss
        bossCamera.Priority = 20;
        playerCamera.Priority = 5;

        yield return new WaitForSeconds(focusTime + pauseTime);

        // 🎥 Volver al jugador
        bossCamera.Priority = 0;
        playerCamera.Priority = 20;

        yield return new WaitForSeconds(returnTime);

        GameStateManager.Instance?.SetState(GameState.Playing);
    }
}
