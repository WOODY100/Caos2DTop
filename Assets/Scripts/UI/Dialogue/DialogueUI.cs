using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    private NPCActionExecutor currentExecutor;

    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text speakerNameText;
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

    public void Show(string speakerName, DialogueLine[] lines, NPCActionExecutor executor)
    {
        if (lines == null || lines.Length == 0)
            return;

        speakerNameText.text = speakerName;
        speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speakerName));

        currentLines = lines;
        currentExecutor = executor;
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

        if (line.giveItem && currentExecutor != null)
        {
            GiveItemFromLine(line);
        }
    }

    private void GiveItemFromLine(DialogueLine line)
    {
        ItemData item = ItemDatabase.Instance.GetItem(line.itemID);

        if (item == null)
        {
            Debug.LogError($"Item NO encontrado: {line.itemID}");
            return;
        }

        if (!string.IsNullOrEmpty(line.giveOnceFlag))
        {
            currentExecutor.GiveItemOnce(item, line.amount, line.giveOnceFlag);
        }
        else
        {
            currentExecutor.GiveItem(item, line.amount);
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
