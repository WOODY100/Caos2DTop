using UnityEngine;

public class NPCInkDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcID;
    [SerializeField] private TextAsset inkFile;
    [SerializeField] private InkDialogueUI dialogueUI;

    public void Interact()
    {
        if (InkManager.Instance.IsRunning)
            return;

        string startKnot = InkDialogueStateSelector.GetStartKnot(npcID);

        InkManager.Instance.StartStory(inkFile, startKnot);
        dialogueUI.Open();
    }
}
