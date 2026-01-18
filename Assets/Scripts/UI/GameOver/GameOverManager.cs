using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    private GameOverUI currentUI;
    private PlayerHealth playerHealth;

    [Header("Cinematic Timing")]
    [SerializeField] private float deathDelay = 0.8f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied -= HandlePlayerDeath;
            playerHealth.OnPlayerDied += HandlePlayerDeath;
        }
    }

    public void RegisterUI(GameOverUI ui)
    {
        currentUI = ui;
    }

    private void HandlePlayerDeath()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        // ⛔ Bloquear gameplay (sin pausar tiempo)
        GameStateManager.Instance.SetState(GameState.GameOver);

        // 🕒 Delay cinematográfico
        yield return new WaitForSeconds(deathDelay);

        // 🌑 Fade Out (pantalla queda negra)
        if (FadeManager.Instance != null)
            yield return FadeManager.Instance.FadeOut();

        // 🖥️ Mostrar UI SOBRE negro
        currentUI?.Show();
    }


}
