using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject continueButton;
    public GameObject newGameButton;

    [Header("Panels")]
    public GameObject saveListPanel;

    private void Start()
    {
        RefreshMenuState();
    }

    public void RefreshMenuState()
    {
        bool hasSaves = SaveManager.Instance.HasAnySave();

        // CONTINUAR
        continueButton.SetActive(hasSaves);

        // LISTA DE SAVES (solo si hay partidas)
        saveListPanel.SetActive(hasSaves);
    }

    public void OnContinuePressed()
    {
        SaveManager.Instance.LoadLastSave();
    }

    public void OnNewGamePressed()
    {
        saveListPanel.SetActive(true);
    }
}