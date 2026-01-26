using System;
using UnityEngine;

public class WorldInitializer : MonoBehaviour
{
    public static event Action OnWorldReady;

    private void Awake()
    {
        if (UIModalManager.Instance != null)
            UIModalManager.Instance.ResetModal();

        if (GamePauseManager.Instance != null)
            GamePauseManager.Instance.ResetAllPauses();
    }

    private void Start()
    {
        StartCoroutine(NotifyWorldReady());
    }

    private System.Collections.IEnumerator NotifyWorldReady()
    {
        yield return null; // 1 frame
        yield return null; // 2 frames (seguridad)

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Playing);

        OnWorldReady?.Invoke();
    }
}