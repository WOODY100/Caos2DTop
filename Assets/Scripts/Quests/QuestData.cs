using UnityEngine;

public enum QuestType
{
    Main,
    Side
}

[CreateAssetMenu(menuName = "Quests/Quest")]
public class QuestData : ScriptableObject
{
    [Header("Info")]
    public string questID;
    public string questName;

    [TextArea(3, 6)]
    public string description;

    [Header("Type")]
    public QuestType questType = QuestType.Side;

    [Header("Completion")]
    public string[] requiredFlags;

    [Header("Optional")]
    public bool autoComplete = true;
}