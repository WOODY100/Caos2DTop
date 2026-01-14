using UnityEngine;

public class GameStateTester : MonoBehaviour
{
    void Start()
    {
        if (GameStateManager.Instance == null)
        {
            Debug.LogError("GameStateManager no existe");
            return;
        }

        GameStateManager.Instance.OnStateChanged += OnStateChanged;

        GameStateManager.Instance.SetState(GameState.Paused);
        GameStateManager.Instance.SetState(GameState.Playing);
    }

    void OnDestroy()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnStateChanged -= OnStateChanged;
    }

    void OnStateChanged(GameState state)
    {
        Debug.Log("GameState cambiado a: " + state);
    }
}
