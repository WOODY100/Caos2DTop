using UnityEngine;
using Unity.Cinemachine;

public class CameraTransitionController : MonoBehaviour
{
    public static CameraTransitionController Instance;

    private CinemachineCamera vcam;
    private Transform followTarget;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        CacheCamera();
    }

    void CacheCamera()
    {
        if (vcam == null)
            vcam = FindFirstObjectByType<CinemachineCamera>();

        if (vcam != null)
            followTarget = vcam.Follow;
    }

    /// <summary>
    /// Notifica a Cinemachine que el target fue teletransportado
    /// </summary>
    public void NotifyTeleport(Vector3 oldPos, Vector3 newPos)
    {
        if (vcam == null || followTarget == null)
            CacheCamera();

        if (vcam == null || followTarget == null)
            return;

        Vector3 delta = newPos - oldPos;
        vcam.OnTargetObjectWarped(followTarget, delta);
    }
}
