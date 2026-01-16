using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string text;

    [Header("Optional WorldState")]
    public string flagOnShow;
}

public class NPCDialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string requiredFlag;
        public DialogueLine[] lines;
    }

    [SerializeField] private DialogueEntry[] dialogues;

    public void StartDialogue(string speakerName)
    {
        foreach (var entry in dialogues)
        {
            if (string.IsNullOrEmpty(entry.requiredFlag) ||
                WorldStateManager.Instance.HasFlag(entry.requiredFlag))
            {
                DialogueUI.Instance.Show(speakerName, entry.lines);
                return;
            }
        }
    }
}
