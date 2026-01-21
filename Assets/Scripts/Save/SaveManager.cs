using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public const int MaxSlots = 5;
    public const int CURRENT_SAVE_VERSION = 1;

    // ======================
    // WORLD FLAGS
    // ======================
    public List<string> worldFlags = new();

    public int CurrentSlot { get; private set; } = -1;
    public bool IsLoadingFromSave { get; private set; }


    public bool IsLoading { get; private set; }
    public static SaveManager Instance;
    private string SaveDirectory =>
    Path.Combine(Application.persistentDataPath, "Saves");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // =========================
    // SAVE
    // =========================
    public bool SlotExists(int slot)
    {
        return File.Exists(GetSavePath(slot));
    }

    private string GetSavePath(int slot)
    {
        if (!Directory.Exists(SaveDirectory))
            Directory.CreateDirectory(SaveDirectory);

        return Path.Combine(SaveDirectory, $"save_{slot}.json");
    }

    public void SaveGameToSlot(int slot)
    {
        CurrentSlot = slot;

        SaveData data = BuildSaveData();
        if (data == null) return;

        // 🔹 VERSIONADO
        data.saveVersion = CURRENT_SAVE_VERSION;

        // -------------------------
        // METADATA
        // -------------------------
        PlayerExperience exp = FindFirstObjectByType<PlayerExperience>();

        data.meta = new SaveMetaData
        {
            saveName = $"Partida {slot + 1}",
            sceneName = data.sceneName,
            playerLevel = exp != null ? exp.level : 1,
            lastPlayed = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm")
        };


        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(slot), json);

        if (SaveFeedbackUI.Instance != null)
            SaveFeedbackUI.Instance.Show("Juego guardado...");
    }

    public void DeleteSave(int slot)
    {
        string path = GetSavePath(slot);

        if (File.Exists(path))
        {
            File.Delete(path);
            //Debug.Log($"Save del slot {slot} eliminado");
        }
    }

    // =========================
    // LOAD
    // =========================
    public void LoadGame(int slot)
    {
        CurrentSlot = slot;
        IsLoadingFromSave = true;

        if (IsLoading)
            return;

        string path = GetSavePath(slot);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"No existe save en slot {slot}");
            return;
        }

        IsLoading = true;

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // 🔹 VERSIONADO
        if (data.saveVersion < CURRENT_SAVE_VERSION)
        {
            UpgradeSaveData(data);

            // 🔹 Reescribir save actualizado
            string upgradedJson = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, upgradedJson);
        }

        StartCoroutine(LoadRoutine(data));
    }

    private IEnumerator LoadRoutine(SaveData data)
    {
        // 🔒 Bloquear gameplay
        GameStateManager.Instance?.SetState(GameState.Transition);

        // 🌑 Fade Out
        yield return FadeManager.Instance.FadeOut();

        // 🗺️ Cargar escena (SIN frame intermedio)
        yield return SceneManager.LoadSceneAsync(data.sceneName);

        // ⛔ Cortar seguimiento ANTES de cualquier cálculo
        CameraTransitionController.Instance?.DisableFollow();

        // 💾 Aplicar datos (posición / spawn / stats)
        foreach (var saveable in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (saveable is ISaveable s)
                s.LoadData(data);
        }

        // 🔔 Notificar teleport FINAL
        var player = FindFirstObjectByType<PlayerController>();
        CameraTransitionController.Instance?.NotifyTeleport(
            Vector3.zero,
            player.transform.position
        );

        // ✅ Activar seguimiento YA correcto
        CameraTransitionController.Instance?.EnableFollow();

        // 🔄 Recalcular stats
        FindFirstObjectByType<PlayerStats>()?.RecalculateStats();

        // 🖥️ UI
        FindFirstObjectByType<InventoryHUD>()?.Refresh();

        // 🌕 Fade In
        yield return FadeManager.Instance.FadeIn();

        // ▶ Gameplay
        GameStateManager.Instance?.SetState(GameState.Playing);

        IsLoadingFromSave = false;
        IsLoading = false;
    }


    private SaveData BuildSaveData()
    {
        SaveData data = new SaveData
        {
            sceneName = SceneManager.GetActiveScene().name
        };

        foreach (var saveable in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (saveable is ISaveable s)
                s.SaveData(data);
        }

        return data;
    }

    public List<(int slot, SaveMetaData meta)> GetAllSaves()
    {
        List<(int, SaveMetaData)> result = new();

        if (!Directory.Exists(SaveDirectory))
            return result;

        string[] files = Directory.GetFiles(SaveDirectory, "save_*.json");

        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                if (data == null || data.meta == null)
                    continue;

                string fileName = Path.GetFileNameWithoutExtension(file);
                string slotString = fileName.Replace("save_", "");

                if (!int.TryParse(slotString, out int slot))
                    continue;

                result.Add((slot, data.meta));
            }
            catch
            {
                Debug.LogWarning($"No se pudo leer el save: {file}");
            }
        }

        return result;
    }

    public bool HasAnySave()
    {
        if (!Directory.Exists(SaveDirectory))
            return false;

        return Directory.GetFiles(SaveDirectory, "save_*.json").Length > 0;
    }

    public void LoadLastSave()
    {
        var saves = GetAllSaves();

        if (saves.Count == 0)
        {
            Debug.LogWarning("No hay partidas para continuar");
            return;
        }

        // Buscar el save más reciente
        int latestSlot = -1;
        System.DateTime latestTime = System.DateTime.MinValue;

        foreach (var save in saves)
        {
            if (System.DateTime.TryParse(save.meta.lastPlayed, out var time))
            {
                if (time > latestTime)
                {
                    latestTime = time;
                    latestSlot = save.slot;
                }
            }
        }

        if (latestSlot == -1)
        {
            Debug.LogWarning("No se pudo determinar el último save");
            return;
        }

        LoadGame(latestSlot);
    }

    public void CreateNewGame(int slot)
    {
        CurrentSlot = slot;

        // Crear un SaveData LIMPIO (estado inicial)
        SaveData data = new SaveData
        {
            saveVersion = CURRENT_SAVE_VERSION,

            sceneName = "World",
            playerPosition = Vector3.zero,

            // 🔹 NIVEL
            playerLevel = 1,
            playerCurrentExp = 0,
            playerExpToNextLevel = 100,

            // 🔹 BASE STATS INICIALES (CLAVE)
            baseHealth = 100,
            baseAttack = 10,
            baseDefense = 5,
            baseSpeed = 5f,
            currentHealth = 100,

            inventoryItems = new(),
            equippedItems = new(),
            coins = 0,

            meta = new SaveMetaData
            {
                saveName = $"Partida {slot + 1}",
                sceneName = "World",
                playerLevel = 1,
                lastPlayed = System.DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            }
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetSavePath(slot), json);
        SpawnManager.Instance?.SetSpawn("start");

        LoadGame(slot);
        
        PlayerStats stats = FindFirstObjectByType<PlayerStats>();
        stats?.InitializeNewGame();
    }

    public void SaveCurrentGame()
    {
        if (CurrentSlot < 0)
            return;

        SaveGameToSlot(CurrentSlot);
    }

    private void UpgradeSaveData(SaveData data)
    {
        // 🟡 EJEMPLO: versión 0 → versión 1
        if (data.saveVersion == 0)
        {
            // Valores por defecto para stats base
            if (data.baseHealth <= 0) data.baseHealth = 100;
            if (data.baseAttack <= 0) data.baseAttack = 10;
            if (data.baseDefense <= 0) data.baseDefense = 5;
            if (data.baseSpeed <= 0) data.baseSpeed = 5f;

            data.saveVersion = 1;
        }

        if (data.saveVersion == 1)
        {
            if (data.currentHealth <= 0)
                data.currentHealth = data.baseHealth;

            data.saveVersion = 2;
        }

        // 🔮 Futuras versiones aquí
        // if (data.saveVersion == 1) { ... data.saveVersion = 2; }
    }

}
