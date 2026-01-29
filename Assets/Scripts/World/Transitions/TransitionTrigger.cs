using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class TransitionTrigger : MonoBehaviour, IInteractable
{
    public enum TransitionType
    {
        SameScene,
        LoadScene
    }

    public enum LightMode
    {
        None,
        EnterDark,
        ExitDark
    }


    [Header("Lighting")]
    public LightMode lightMode = LightMode.None;

    public float targetLightIntensity = 0.15f;
    public float lightTransitionDuration = 0.5f;
    public float exteriorLightIntensity = 1f;

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
    Light2D globalLight;
    float originalLightIntensity;

    void Awake()
    {
        Light2D[] lights = FindObjectsByType<Light2D>(FindObjectsSortMode.None);

        foreach (var light in lights)
        {
            if (light.lightType == Light2D.LightType.Global)
            {
                globalLight = light;
                break;
            }
        }

        if (globalLight == null)
        {
            Debug.LogError(
                "[TransitionTrigger] No se encontró Global Light 2D en la escena",
                this
            );
        }
    }


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

            // 🌑 / ☀️ Cambiar iluminación según el trigger
            if (globalLight != null)
            {
                if (lightMode == LightMode.EnterDark)
                {
                    yield return LerpGlobalLight(
                        globalLight.intensity,
                        targetLightIntensity
                    );
                }
                else if (lightMode == LightMode.ExitDark)
                {
                    yield return LerpGlobalLight(
                        globalLight.intensity,
                        exteriorLightIntensity
                    );
                }
            }

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

    IEnumerator LerpGlobalLight(float from, float to)
    {
        if (globalLight == null)
            yield break;

        float t = 0f;

        while (t < lightTransitionDuration)
        {
            t += Time.unscaledDeltaTime;
            globalLight.intensity = Mathf.Lerp(from, to, t / lightTransitionDuration);
            yield return null;
        }

        globalLight.intensity = to;
    }
}
