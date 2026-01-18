using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    void Awake()
    {
        panel.SetActive(false);
        GameOverManager.Instance?.RegisterUI(this);
    }

    public void Show()
    {
        panel.SetActive(true);
    }

    public void OnRetry()
    {
        // ⛔ Ocultar Game Over inmediatamente
        panel.SetActive(false);

        // 🔄 Estado correcto (no Playing)
        GameStateManager.Instance.SetState(GameState.Transition);

        // 💾 Cargar último save
        SaveManager.Instance.LoadLastSave();
    }


    public void OnMainMenu()
    {
        panel.SetActive(false);

        GameStateManager.Instance.SetState(GameState.Menu);

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
