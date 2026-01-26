using UnityEngine;
using UnityEngine.InputSystem;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform content;
    [SerializeField] private QuestItemUI questItemPrefab;

    private void Start()
    {
        panel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Toggle();
        }
    }

    private void OnEnable()
    {
        QuestManager.OnQuestStatusChanged += Refresh;
    }

    private void OnDisable()
    {
        QuestManager.OnQuestStatusChanged -= Refresh;
    }

    public void Toggle()
    {
        bool newState = !panel.activeSelf;
        panel.SetActive(newState);

        if (newState)
            Refresh();
    }

    private void Refresh()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        foreach (var quest in QuestManager.Instance.GetAllQuests())
        {
            QuestItemUI item = Instantiate(questItemPrefab, content);
            item.Setup(quest);
        }
    }
}
