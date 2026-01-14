using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public bool IsLoading { get; private set; }
    public static SaveManager Instance;
    private string SaveDirectory =>
    Path.Combine(Application.persistentDataPath, "Saves");

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

    public const int MaxSlots = 5;

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
        SaveData data = BuildSaveData();
        if (data == null) return;

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
    }


    public bool HasSave()
    {
        return File.Exists(SavePath);
    }

    public void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            //Debug.Log("Archivo de guardado eliminado");
        }
    }

    public void DeleteSave(int slot)
    {
        string path = GetSavePath(slot);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Save del slot {slot} eliminado");
        }
    }


    public void SaveGame()
    {
        SaveData data = new SaveData();

        // -------------------------
        // SCENE
        // -------------------------
        data.sceneName = SceneManager.GetActiveScene().name;

        // -------------------------
        // PLAYER POSITION
        // -------------------------
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            data.playerPosition = player.transform.position;

        // -------------------------
        // INVENTORY
        // -------------------------
        data.inventoryItemIDs.Clear();

        foreach (var item in InventoryManager.Instance.items)
        {
            if (item != null)
                data.inventoryItemIDs.Add(item.itemID);
        }

        // -------------------------
        // EQUIPMENT
        // -------------------------
        data.equippedItems.Clear();

        foreach (var pair in EquipmentManager.Instance.GetEquippedDictionary())
        {
            if (pair.Value == null) continue;

            data.equippedItems.Add(new EquippedItemData
            {
                slotType = pair.Key,
                itemID = pair.Value.itemID
            });
        }

        // -------------------------
        // CURRENCY
        // -------------------------
        data.coins = InventoryManager.Instance.GetCoins();
        data.keys = InventoryManager.Instance.GetKeys();

        // -------------------------
        // WRITE FILE
        // -------------------------
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    // =========================
    // LOAD
    // =========================
    public void LoadGame()
    {
        if (IsLoading)
            return;

        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No existe archivo de guardado");
            return;
        }

        IsLoading = true;

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        StartCoroutine(LoadRoutine(data));
    }

    public void LoadGame(int slot)
    {
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

        StartCoroutine(LoadRoutine(data));
    }


    private IEnumerator LoadRoutine(SaveData data)
    {
        // 🔒 Bloquear gameplay
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Transition);

        // 🌑 FADE OUT
        if (FadeManager.Instance != null)
            yield return FadeManager.Instance.FadeOut();

        // -------------------------
        // LOAD SCENE
        // -------------------------
        yield return SceneManager.LoadSceneAsync(data.sceneName);
        yield return null;

        // -------------------------
        // PLAYER POSITION + CAMERA TELEPORT
        // -------------------------
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 oldPos = player.transform.position;
            Vector3 newPos = data.playerPosition;

            player.transform.position = newPos;

            if (CameraTransitionController.Instance != null)
                CameraTransitionController.Instance.NotifyTeleport(oldPos, newPos);
        }

        // -------------------------
        // INVENTORY (PRIMERO)
        // -------------------------
        InventoryManager.Instance.items.Clear();

        foreach (string id in data.inventoryItemIDs)
        {
            ItemData item = ItemDatabase.Instance.GetItem(id);
            if (item != null)
                InventoryManager.Instance.AddItem(item, 1);
        }

        // -------------------------
        // EQUIPMENT (DESPUÉS)
        // -------------------------
        EquipmentManager.Instance.ClearAllSlots();

        foreach (var equipped in data.equippedItems)
        {
            ItemData item = ItemDatabase.Instance.GetItem(equipped.itemID);
            if (item != null)
                EquipmentManager.Instance.EquipSilently(item);
        }

        // -------------------------
        // RECALCULAR STATS (UNA VEZ)
        // -------------------------
        PlayerStats stats = Object.FindFirstObjectByType<PlayerStats>();
        if (stats != null)
            stats.RecalculateStats();

        // -------------------------
        // REFRESH UI
        // -------------------------
        InventoryHUD hud = Object.FindFirstObjectByType<InventoryHUD>();
        if (hud != null)
            hud.Refresh();

        // 🌕 FADE IN

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.SetAlpha(1f); // asegura negro
            yield return null;                 // deja renderizar un frame
            yield return FadeManager.Instance.FadeIn();
        }

        // ▶ RESUME GAMEPLAY
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Playing);

        IsLoading = false;
    }

    private SaveData BuildSaveData()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogError("BuildSaveData llamado fuera de gameplay");
            return null;
        }

        SaveData data = new SaveData();

        data.sceneName = SceneManager.GetActiveScene().name;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            data.playerPosition = player.transform.position;

        data.inventoryItemIDs.Clear();
        foreach (var item in InventoryManager.Instance.items)
            if (item != null)
                data.inventoryItemIDs.Add(item.itemID);

        data.equippedItems.Clear();
        foreach (var pair in EquipmentManager.Instance.GetEquippedDictionary())
        {
            if (pair.Value == null) continue;
            data.equippedItems.Add(new EquippedItemData
            {
                slotType = pair.Key,
                itemID = pair.Value.itemID
            });
        }

        data.coins = InventoryManager.Instance.GetCoins();
        data.keys = InventoryManager.Instance.GetKeys();

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
        // Crear un SaveData LIMPIO (estado inicial)
        SaveData data = new SaveData
        {
            sceneName = "World", // ⚠️ tu escena inicial
            playerPosition = Vector3.zero,
            inventoryItemIDs = new List<string>(),
            equippedItems = new List<EquippedItemData>(),
            coins = 0,
            keys = 0,
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

        // Cargar directamente esa partida nueva
        LoadGame(slot);
    }
}
