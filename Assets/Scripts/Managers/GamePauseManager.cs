using UnityEngine;
using System.Collections.Generic;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager Instance { get; private set; }

    private HashSet<object> pauseRequests = new();
    private PlayerController player;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
    }

    public void RequestPause(object requester)
    {
        pauseRequests.Add(requester);
        UpdateState();
    }

    public void ReleasePause(object requester)
    {
        pauseRequests.Remove(requester);
        UpdateState();
    }

    void UpdateState()
    {
        bool paused = pauseRequests.Count > 0;

        // ⏸ Pausa lógica
        Time.timeScale = paused ? 0f : 1f;

        // 🔒 Bloqueo TOTAL de input
        if (player != null)
            player.SetInputEnabled(!paused);
    }

    // 🔍 Para que otras UIs consulten
    public bool IsPaused()
    {
        return pauseRequests.Count > 0;
    }

    public bool IsPausedBy<T>()
    {
        foreach (var req in pauseRequests)
        {
            if (req is T) return true;
        }
        return false;
    }


}
