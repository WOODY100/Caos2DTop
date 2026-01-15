using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private string npcID;
    [SerializeField] private string npcName;

    private NPCDialogue dialogue;

    protected virtual void Awake()
    {
        dialogue = GetComponent<NPCDialogue>();
    }

    public virtual void Interact()
    {
        dialogue?.StartDialogue(npcName);
    }
}
