using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    private bool isOpen;
    private bool isExiting;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (isExiting) return;

        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if (isOpen || isExiting) return;

        if (UIModalManager.Instance != null &&
        !UIModalManager.Instance.RequestOpen(this))
            return;

        isOpen = true;
        gameObject.SetActive(true);
        GamePauseManager.Instance.RequestPause(this);
    }

    public void Close()
    {
        if (!isOpen || isExiting) return;

        isOpen = false;
        gameObject.SetActive(false);
        GamePauseManager.Instance.ReleasePause(this);
        
        if (UIModalManager.Instance != null)
            UIModalManager.Instance.Close(this);

    }

    public void Resume()
    {
        Close();
    }

    public void SaveAndExit()
    {
        StartCoroutine(SaveAndExitRoutine());
    }

    private IEnumerator SaveAndExitRoutine()
    {
        isExiting = true; // 🔒 BLOQUEAR INPUT DEL MENÚ

        // 1️⃣ Guardar
        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveCurrentGame();

        // 2️⃣ Esperar feedback visual
        if (SaveFeedbackUI.Instance != null)
        {
            yield return new WaitForSecondsRealtime(
                SaveFeedbackUI.Instance.TotalDuration
            );
        }

        // 3️⃣ Liberar pausa
        if (GamePauseManager.Instance != null)
            GamePauseManager.Instance.ReleasePause(this);

        // 4️⃣ Cambiar estado global
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Transition);

        // 5️⃣ Ocultar menú
        gameObject.SetActive(false);

        // 🔥 LIMPIEZA GLOBAL (CLAVE)
        if (UIModalManager.Instance != null)
            UIModalManager.Instance.ResetModal();

        if (GamePauseManager.Instance != null)
            GamePauseManager.Instance.ResetAllPauses();

        // 6️⃣ Cambiar escena
        SceneManager.LoadScene("MainMenu");

    }

    public void Save()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveCurrentGame();
    }

}
