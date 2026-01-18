using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestManager : MonoBehaviour, ISaveable
{
    public static event Action OnQuestStatusChanged;
    public static QuestManager Instance;

    bool anyCompleted = false;

    [SerializeField] private List<QuestData> allQuests = new();

    private HashSet<string> completedQuests = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        WorldStateManager.OnWorldStateChanged += EvaluateQuests;
    }

    private void Update()
    {
        EvaluateQuests();
    }

    private void EvaluateQuests()
    {
        foreach (var quest in allQuests)
        {
            if (completedQuests.Contains(quest.questID))
                continue;

            if (IsQuestCompleted(quest))
            {
                completedQuests.Add(quest.questID);
                Debug.Log($"[QUEST COMPLETED] {quest.questName}");
                anyCompleted = true;
            }
        }
        
        if (anyCompleted)
            OnQuestStatusChanged?.Invoke();
    }

    private bool IsQuestCompleted(QuestData quest)
    {
        foreach (var flag in quest.requiredFlags)
        {
            if (!WorldStateManager.Instance.HasFlag(flag))
                return false;
        }
        return true;
    }

    public bool IsCompleted(QuestData quest)
    {
        return completedQuests.Contains(quest.questID);
    }

    public IEnumerable<QuestData> GetAllQuests() => allQuests;

    public void SaveData(SaveData data)
    {
        data.completedQuestIDs.Clear();
        data.completedQuestIDs.AddRange(completedQuests);
    }

    public void LoadData(SaveData data)
    {
        completedQuests.Clear();

        if (data.completedQuestIDs != null)
        {
            foreach (var id in data.completedQuestIDs)
                completedQuests.Add(id);
        }

        OnQuestStatusChanged?.Invoke();
    }
}
