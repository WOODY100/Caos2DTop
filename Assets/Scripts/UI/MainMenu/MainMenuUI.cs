using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    void Start()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Menu);
    }

    public void ContinueGame()
    {
        if (SaveManager.Instance == null)
        {
            Debug.LogError("SaveManager no existe");
            return;
        }

        SaveManager.Instance.LoadGame();
    }

    public void NewGame()
    {
        SaveManager.Instance.DeleteSave();

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
