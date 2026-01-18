using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    void Start()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Menu);

        // 🔑 FIX CLAVE: revelar menú si venimos de negro
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.SetAlpha(1f);      // asegurar negro
            StartCoroutine(FadeManager.Instance.FadeIn());
        }
    }

    public void ContinueGame()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager no existe");
            return;
        }

        SaveManager.Instance.LoadLastSave();
    }

    public void NewGame()
    {
        GameStateManager.Instance.SetState(GameState.Transition);

        SceneManager.LoadScene("World");

        // 👇 CLAVE: activar gameplay tras cargar escena
        StartCoroutine(SetPlayingNextFrame());
    }

    private IEnumerator SetPlayingNextFrame()
    {
        yield return null; // esperar 1 frame
        GameStateManager.Instance.SetState(GameState.Playing);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
