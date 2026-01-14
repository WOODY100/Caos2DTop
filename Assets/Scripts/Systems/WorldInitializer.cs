using UnityEngine;

public class WorldInitializer : MonoBehaviour
{
    private void Start()
    {
        // 🔑 ESTE ES EL MOMENTO CORRECTO
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SetState(GameState.Playing);
        }
    }
}
