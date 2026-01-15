using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueEntry
    {
        public string requiredFlag;
        [TextArea(2, 5)]
        public string[] lines;
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
