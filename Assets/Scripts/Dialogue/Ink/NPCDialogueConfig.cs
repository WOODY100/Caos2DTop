using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(
    fileName = "NPCDialogueConfig",
    menuName = "Dialogue/NPC Dialogue Config"
)]
public class NPCDialogueConfig : ScriptableObject
{
    public string npcID;

    [Tooltip("Reglas evaluadas en orden")]
    public List<DialogueStartRule> startRules = new();

    [Tooltip("Knot por defecto si todas las reglas se cumplen")]
    public string defaultKnot;
}
