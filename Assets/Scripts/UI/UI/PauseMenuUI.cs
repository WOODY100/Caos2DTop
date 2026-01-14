using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    private bool isOpen;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if (isOpen) return;

        isOpen = true;
        gameObject.SetActive(true);
        GamePauseManager.Instance.RequestPause(this);
    }

    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;
        gameObject.SetActive(false);
        GamePauseManager.Instance.ReleasePause(this);
    }

    public void Resume()
    {
        Close();
    }

    public void Save()
    {
        SaveManager.Instance.SaveGame();
    }

    public void Load()
    {
        SaveManager.Instance.LoadGame();
        Close();
    }

    public void ExitToMenu()
    {
        // 1️⃣ Cambiar estado global
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Transition);

        // 2️⃣ Cerrar este menú (evita input fantasma)
        gameObject.SetActive(false);

        // 3️⃣ Cargar menú principal
        SceneManager.LoadScene("MainMenu");
    }
}
