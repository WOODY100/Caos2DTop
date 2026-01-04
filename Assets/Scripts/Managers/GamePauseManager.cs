using UnityEngine;
using System.Collections.Generic;

public class GamePauseManager : MonoBehaviour
{
    public static GamePauseManager Instance { get; private set; }

    private HashSet<object> pauseRequests = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RequestPause(object requester)
    {
        pauseRequests.Add(requester);
        UpdatePauseState();
    }

    public void ReleasePause(object requester)
    {
        pauseRequests.Remove(requester);
        UpdatePauseState();
    }

    void UpdatePauseState()
    {
        Time.timeScale = pauseRequests.Count > 0 ? 0f : 1f;
    }
}
