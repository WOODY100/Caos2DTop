using UnityEngine;

public class WorldInitializer : MonoBehaviour
{
    private void Awake()
    {
        // 🔒 RESET DEFENSIVO DE UI / PAUSA
        if (UIModalManager.Instance != null)
            UIModalManager.Instance.ResetModal();

        if (GamePauseManager.Instance != null)
            GamePauseManager.Instance.ResetAllPauses();
    }

    private void Start()
    {
        // ▶️ Entrar a gameplay limpio
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Playing);
    }
}
