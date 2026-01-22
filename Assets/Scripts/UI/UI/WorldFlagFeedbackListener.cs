using UnityEngine;
using System.Collections.Generic;

public class WorldFlagFeedbackListener : MonoBehaviour
{
    [System.Serializable]
    public class FlagMessage
    {
        public string flagID;
        [TextArea]
        public string message;
    }

    [SerializeField] private List<FlagMessage> messages;
    [SerializeField] private Transform player;

    private HashSet<string> shownFlags = new();

    private void Awake()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerController>()?.transform;
    }

    private void OnEnable()
    {
        WorldStateManager.OnWorldStateChanged += OnWorldStateChanged;
    }

    private void OnDisable()
    {
        WorldStateManager.OnWorldStateChanged -= OnWorldStateChanged;
    }

    private void OnWorldStateChanged()
    {
        if (player == null || FeedbackPopupUI.Instance == null)
            return;

        foreach (var entry in messages)
        {
            if (shownFlags.Contains(entry.flagID))
                continue;

            if (WorldStateManager.Instance.HasFlag(entry.flagID))
            {
                shownFlags.Add(entry.flagID);
                FeedbackPopupUI.Instance.Show(entry.message, player.position);
            }
        }
    }
}
