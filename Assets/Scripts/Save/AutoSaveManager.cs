using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSaveManager : MonoBehaviour
{
    public static AutoSaveManager Instance;

    [Header("Autosave Settings")]
    public bool autosaveOnSceneLoad = true;
    public bool autosaveOnTimer = true;
    public float autosaveInterval = 180f; // 3 minutos

    [Header("Autosave Flags")]
    [SerializeField]
    private List<string> autosaveFlags = new()
    {
        "quest_completed",
        "boss_defeated",
        "boss_room_entered",
        "zone_entered"
    };

    [Header("Autosave Cooldown")]
    [SerializeField] private float autosaveCooldown = 30f; // segundos

    private float lastAutoSaveTime = -999f;

    private Coroutine autosaveRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        WorldInitializer.OnWorldReady += HandleWorldReady;
        WorldStateManager.OnFlagSet += HandleFlagSet;
    }

    private void OnDisable()
    {
        WorldInitializer.OnWorldReady -= HandleWorldReady;
        WorldStateManager.OnFlagSet -= HandleFlagSet;
    }

    private void Start()
    {
        if (autosaveOnTimer)
            autosaveRoutine = StartCoroutine(AutoSaveTimer());
    }

    private bool CanAutoSave()
    {
        if (Time.unscaledTime - lastAutoSaveTime < autosaveCooldown)
            return false;

        return true;
    }

    public void ForceAutoSave(string reason)
    {
        //PARA SU USO: AutoSaveManager.Instance.ForceAutoSave("BossDefeated");

        lastAutoSaveTime = -999f;
        TryAutoSave(reason);
    }


    //--------------------------
    // HANDLERS
    //--------------------------
    private void HandleFlagSet(string flagID)
    {
        if (ShouldAutoSaveOnFlag(flagID))
        {
            TryAutoSave($"Flag:{flagID}");
        }
    }
    
    private void HandleWorldReady()
    {
        TryAutoSave("WorldReady");
    }

    // -------------------------
    // EVENTOS
    // -------------------------

    private IEnumerator AutoSaveTimer()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(autosaveInterval);
            TryAutoSave("Timer");
        }
    }

    //--------------------------
    // FLAGS
    //--------------------------

    private bool ShouldAutoSaveOnFlag(string flagID)
    {
        foreach (var key in autosaveFlags)
        {
            if (flagID.StartsWith(key))
                return true;
        }
        return false;
    }

    // -------------------------
    // CORE
    // -------------------------

    public void TryAutoSave(string reason)
    {
        if (SaveManager.Instance == null)
            return;

        if (SaveManager.Instance.CurrentSlot < 0)
            return;

        if (SaveManager.Instance.IsLoading)
            return;

        if (GameStateManager.Instance != null &&
            GameStateManager.Instance.CurrentState != GameState.Playing)
            return;

        if (!CanAutoSave())
        {
            Debug.Log($"[AUTOSAVE] Cooldown activo, ignorado ({reason})");
            return;
        }

        SaveManager.Instance.SaveCurrentGame();
        lastAutoSaveTime = Time.unscaledTime;

        Debug.Log($"[AUTOSAVE] Guardado automático ({reason})");
    }
}