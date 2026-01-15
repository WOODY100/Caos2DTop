using UnityEngine;
using Unity.Cinemachine;

public class CameraTransitionController : MonoBehaviour
{
    public static CameraTransitionController Instance;

    private CinemachineCamera vcam;
    private Transform followTarget;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CacheCamera();
    }

    void CacheCamera()
    {
        if (vcam == null)
            vcam = FindFirstObjectByType<CinemachineCamera>();

        if (vcam != null && followTarget == null)
            followTarget = vcam.Follow;
    }

    // ⛔ Evita que Cinemachine calcule movimiento
    public void DisableFollow()
    {
        CacheCamera();
        if (vcam != null)
            vcam.Follow = null;
    }

    // ✅ Activa seguimiento SOLO cuando ya todo está listo
    public void EnableFollow()
    {
        CacheCamera();
        if (vcam != null && followTarget != null)
            vcam.Follow = followTarget;
    }

    // 🔔 Teleport limpio
    public void NotifyTeleport(Vector3 oldPos, Vector3 newPos)
    {
        CacheCamera();
        if (vcam == null || followTarget == null)
            return;

        Vector3 delta = newPos - oldPos;
        vcam.OnTargetObjectWarped(followTarget, delta);
    }
}
