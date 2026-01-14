using UnityEngine;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text saveNameText;
    public TMP_Text infoText;

    [Header("Buttons")]
    public GameObject loadButton;
    public GameObject deleteButton;
    public GameObject newGameButton;

    private int slotIndex;

    public void Setup(int slot, SaveMetaData meta)
    {
        slotIndex = slot;

        saveNameText.text = meta.saveName;
        infoText.text =
            $"Nivel {meta.playerLevel} - {meta.sceneName}\n{meta.lastPlayed}";
    }

    public void OnLoadPressed()
    {
        SaveManager.Instance.LoadGame(slotIndex);
    }

    public void OnDeletePressed()
    {
        if (ConfirmDialog.Instance == null)
        {
            Debug.LogError("ConfirmDialog no existe o está desactivado en la escena");
            return;
        }

        ConfirmDialog.Instance.Show(
            "Borrar partida",
            "¿Seguro que deseas borrar esta partida?",
            () =>
            {
                SaveManager.Instance.DeleteSave(slotIndex);
                Destroy(gameObject);

                FindFirstObjectByType<MainMenuController>()?.RefreshMenuState();
            }
        );
    }

    public void SetupOccupied(int slot, SaveMetaData meta)
    {
        slotIndex = slot;

        saveNameText.text = meta.saveName;
        infoText.text =
            $"Nivel {meta.playerLevel} - {meta.sceneName}\n{meta.lastPlayed}";

        loadButton.SetActive(true);
        deleteButton.SetActive(true);
        newGameButton.SetActive(false);
    }

    public void SetupEmpty(int slot)
    {
        slotIndex = slot;

        saveNameText.text = $"Slot {slot + 1}";
        infoText.text = "Vacío";

        loadButton.SetActive(false);
        deleteButton.SetActive(false);
        newGameButton.SetActive(true);
    }

    public void OnNewGamePressed()
    {
        ConfirmDialog.Instance.Show(
            "Nueva partida",
            "¿Crear una nueva partida en este slot?",
            () =>
            {
                SaveManager.Instance.CreateNewGame(slotIndex);
            }
        );
    }
}
