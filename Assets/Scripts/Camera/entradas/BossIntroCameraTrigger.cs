using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;


public class BossIntroCameraTrigger : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private Transform bossTarget;
    [SerializeField] private PixelPerfectCamera pixelPerfectCamera;

    [Header("Zoom")]
    [SerializeField] private float bossZoom = 3.5f;
    [SerializeField] private float zoomInTime = 0.6f;
    [SerializeField] private float focusTime = 1.0f;
    [SerializeField] private float zoomOutTime = 0.6f;

    [Header("Boss Framing")]
    [SerializeField] private float bossYOffset = 1.8f; // ajusta a gusto
    [SerializeField] private int pixelsPerUnit = 16;

    bool triggered;

    float originalZoom;
    Transform originalFollow;
    Transform originalLookAt;

    // 🔧 Framing (suavizado de Cinemachine)
    CinemachinePositionComposer composer;
    Vector3 originalDamping;
    Vector3 originalTargetOffset;

    void Awake()
    {
        originalZoom = cinemachineCamera.Lens.OrthographicSize;
        originalFollow = cinemachineCamera.Follow;
        originalLookAt = cinemachineCamera.LookAt;

        // 🎯 Obtener framing
        composer = cinemachineCamera.GetComponent<CinemachinePositionComposer>();

        if (composer != null)
        {
            originalDamping = composer.Damping;
            originalTargetOffset = composer.TargetOffset;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(CameraSequence());
    }

    IEnumerator CameraSequence()
    {
        GameStateManager.Instance?.SetState(GameState.Transition);

        // ⛔ Pixel Perfect OFF
        if (pixelPerfectCamera != null)
            pixelPerfectCamera.enabled = false;

        DisableDamping();

        cinemachineCamera.Follow = bossTarget;
        cinemachineCamera.LookAt = bossTarget;

        composer.TargetOffset = new Vector3(
            originalTargetOffset.x,
            bossYOffset,
            originalTargetOffset.z
        );

        yield return SmoothZoom(originalZoom, bossZoom, zoomInTime);
        yield return new WaitForSeconds(focusTime);
        yield return SmoothZoom(bossZoom, originalZoom, zoomOutTime);

        cinemachineCamera.Follow = originalFollow;
        cinemachineCamera.LookAt = originalLookAt;
        composer.TargetOffset = originalTargetOffset;

        RestoreDamping();

        // ✅ Pixel Perfect ON
        if (pixelPerfectCamera != null)
            pixelPerfectCamera.enabled = true;

        GameStateManager.Instance?.SetState(GameState.Playing);
    }

    IEnumerator SmoothZoom(float from, float to, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            float raw = Mathf.SmoothStep(from, to, t / duration);
            float snapped = SnapOrthoSize(raw);

            cinemachineCamera.Lens.OrthographicSize = snapped;
            yield return null;
        }

        cinemachineCamera.Lens.OrthographicSize = SnapOrthoSize(to);
    }

    void DisableDamping()
    {
        if (composer == null) return;
        composer.Damping = Vector3.zero;
    }

    void RestoreDamping()
    {
        if (composer == null) return;
        composer.Damping = originalDamping;
    }

    float SnapOrthoSize(float size)
    {
        // Tamaño de un pixel en mundo
        float pixelWorldSize = 1f / pixelsPerUnit;

        // OrthographicSize controla mitad de la altura visible
        float snapped = Mathf.Round(size / pixelWorldSize) * pixelWorldSize;
        return snapped;
    }

}
