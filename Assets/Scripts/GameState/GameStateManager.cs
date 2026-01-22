using UnityEngine;
using System;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState CurrentState { get; private set; }

    public event Action<GameState> OnStateChanged;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 🔑 Estado inicial
        SetState(GameState.Boot);
    }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState)
            return;

        CurrentState = newState;

        //Debug.Log("[GameState] → " + newState);

        ApplyStateEffects(newState);

        OnStateChanged?.Invoke(newState);
    }

    private void ApplyStateEffects(GameState state)
    {
        switch (state)
        {
            // 🎮 Gameplay normal
            case GameState.Playing:
                Time.timeScale = 1f;
                EnableGameplay(true);
                break;

            // ⏸️ PAUSA REAL (ÚNICA)
            case GameState.Paused:
                Time.timeScale = 0f;
                EnableGameplay(false);
                break;

            // 📦 OVERLAYS (NO PAUSAN EL MUNDO)
            case GameState.Inventory:
            case GameState.Loot:
                Time.timeScale = 1f;          // 🔥 NO pausar
                //EnableGameplay(false);        // solo bloquea input
                break;

            // 🧠 Estados de decisión (pausan, pero controlados)
            case GameState.LevelUp:
            case GameState.Dialogue:
                Time.timeScale = 0f;
                EnableGameplay(false);
                break;

            // 🔁 Estados especiales
            case GameState.Transition:
            case GameState.Menu:
            case GameState.GameOver:
                Time.timeScale = 1f;
                EnableGameplay(false);
                break;
        }
    }


    private void EnableGameplay(bool enabled)
    {
        // Player
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var controller = player.GetComponent<PlayerController>();
            if (controller != null)
                controller.SetInputEnabled(enabled);
        }
    }

    public bool IsGameplayAllowed()
    {
        return CurrentState == GameState.Playing
            || CurrentState == GameState.Loot;
    }
}
