using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InkDialogueUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text speakerText;
    [SerializeField] private TMP_Text dialogueText;

    private void Awake()
    {
        root.SetActive(false);
    }

    private void OnEnable()
    {
        InkManager.Instance.OnLine += ShowLine;
        InkManager.Instance.OnDialogueEnd += Close;
    }

    private void OnDisable()
    {
        InkManager.Instance.OnLine -= ShowLine;
        InkManager.Instance.OnDialogueEnd -= Close;
    }

    public void Open()
    {
        root.SetActive(true);

        // ⏸️ Pausar juego por diálogo
        GameStateManager.Instance?.SetState(GameState.Dialogue);

        InkManager.Instance.Advance(); // 🔥 PRIMERA LÍNEA
    }


    private void Update()
    {
        if (!root.activeSelf)
            return;

        if (Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            InkManager.Instance.Advance();
        }
    }

    private void ShowLine(string text, string speaker)
    {
        dialogueText.text = text;

        if (!string.IsNullOrEmpty(speaker))
        {
            speakerText.text = speaker;
            speakerText.gameObject.SetActive(true);
        }
        else
        {
            speakerText.gameObject.SetActive(false);
        }
    }

    private void Close()
    {
        root.SetActive(false);

        // ▶️ Volver a gameplay normal
        GameStateManager.Instance?.SetState(GameState.Playing);
    }

}