using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionTrigger : MonoBehaviour, IInteractable
{
    public enum TransitionType
    {
        SameScene,
        LoadScene
    }

    [Header("Transition")]
    public TransitionType transitionType;

    [Header("Same Scene")]
    public Transform teleportTarget;

    [Header("Load Scene")]
    public string sceneName;
    public string spawnID;

    [Header("Interaction")]
    [Tooltip("Si es false, la transición ocurre automáticamente al entrar")]
    public bool requireInput = true;

    bool playerInside;
    bool isTransitioning;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        if (!requireInput)
            StartCoroutine(DoTransition());
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
    }

    // 🔔 Llamado SOLO por PlayerInteractor
    public void Interact()
    {
        if (!requireInput) return;
        if (!playerInside || isTransitioning) return;

        StartCoroutine(DoTransition());
    }

    IEnumerator DoTransition()
    {
        isTransitioning = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerController pc = player.GetComponent<PlayerController>();
        pc?.SetInputEnabled(false);

        // 1️⃣ Fade Out
        if (FadeManager.Instance != null)
            yield return FadeManager.Instance.FadeOut();

        if (transitionType == TransitionType.SameScene)
        {
            Vector3 oldPos = player.transform.position;
            Vector3 newPos = teleportTarget.position;

            // 2️⃣ Teleport
            player.transform.position = newPos;

            // 3️⃣ Notificar a Cinemachine
            if (CameraTransitionController.Instance != null)
                CameraTransitionController.Instance.NotifyTeleport(oldPos, newPos);

            // 4️⃣ Fade In
            if (FadeManager.Instance != null)
                yield return FadeManager.Instance.FadeIn();

            pc?.SetInputEnabled(true);
        }
        else
        {
            // Cambio de escena
            SpawnManager.Instance.SetSpawn(spawnID);
            SceneManager.LoadScene(sceneName);
        }

        isTransitioning = false;
    }
}
