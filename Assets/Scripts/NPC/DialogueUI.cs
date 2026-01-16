using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text speakerNameText; // 🔹 NUEVO
    [SerializeField] private TMP_Text dialogueText;

    private DialogueLine[] currentLines;
    private int currentIndex;
    private bool isOpen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        root.SetActive(false);
    }

    private void Update()
    {
        if (!isOpen)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            NextLine();
        }
    }

    public void Show(string speakerName, DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0)
            return;

        speakerNameText.text = speakerName;
        speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speakerName));

        currentLines = lines;
        currentIndex = 0;

        ShowLine();
        root.SetActive(true);
        isOpen = true;

        GameStateManager.Instance?.SetState(GameState.Dialogue);
    }

    private void ShowLine()
    {
        var line = currentLines[currentIndex];
        dialogueText.text = line.text;

        if (!string.IsNullOrEmpty(line.flagOnShow))
        {
            WorldStateManager.Instance.SetFlag(line.flagOnShow);
        }
    }

    private void NextLine()
    {
        currentIndex++;

        if (currentIndex >= currentLines.Length)
        {
            Close();
            return;
        }

        ShowLine();
    }

    private void Close()
    {
        root.SetActive(false);
        isOpen = false;
        currentLines = null;
        currentIndex = 0;

        GameStateManager.Instance?.SetState(GameState.Playing);
    }
}
