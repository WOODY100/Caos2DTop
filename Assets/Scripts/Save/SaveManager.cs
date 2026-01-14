using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public bool IsLoading { get; private set; }
    public static SaveManager Instance;

    private string SavePath =>
        Path.Combine(Application.persistentDataPath, "save.json");

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
            FadeManager.Instance.SetAlpha(1f); // 🔥 asegura negro
            yield return null;                 // 🔥 deja renderizar un frame
            yield return FadeManager.Instance.FadeIn();

        // ▶ RESUME GAMEPLAY
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Playing);

        IsLoading = false;
    }

}
