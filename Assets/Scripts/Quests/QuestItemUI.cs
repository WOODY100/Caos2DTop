using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text questNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image statusIcon;

    [Header("Icons")]
    [SerializeField] private Sprite iconActive;
    [SerializeField] private Sprite iconCompleted;

    private QuestData quest;

    private void OnEnable()
    {
        QuestManager.OnQuestStatusChanged += Refresh;
    }

    private void OnDisable()
    {
        QuestManager.OnQuestStatusChanged -= Refresh;
    }


    public void Setup(QuestData questData)
    {
        quest = questData;

        questNameText.text = quest.questName;
        descriptionText.text = quest.description;

        Refresh();
    }

    public void Refresh()
    {
        bool completed = QuestManager.Instance.IsCompleted(quest);

        statusIcon.sprite = completed ? iconCompleted : iconActive;
        statusIcon.color = completed ? Color.green : Color.white;
    }
}
