using UnityEngine;
using Ink.Runtime;
using System;

public class InkManager : MonoBehaviour
{
    public static InkManager Instance;

    public Story CurrentStory { get; private set; }

    public event Action<string, string> OnLine;   // texto, speaker
    public event Action OnDialogueEnd;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void BindExternalFunctions()
    {
        // Flags del mundo
        CurrentStory.BindExternalFunction("SetFlag", (string flagID) =>
        {
            WorldStateManager.Instance.SetFlag(flagID);
        });

        // Dar ítems
        CurrentStory.BindExternalFunction("GiveItem", (string itemID, int amount) =>
        {
            var item = ItemDatabase.Instance.GetItem(itemID);
            if (item != null)
            {
                InventoryManager.Instance.AddItem(item, amount);
            }
            else
            {
                Debug.LogError($"[Ink] Item no encontrado: {itemID}");
            }
        });
    }


    // ============================
    // START STORY
    // ============================
    public void StartStory(TextAsset inkFile, string startKnot)
    {
        CurrentStory = new Story(inkFile.text);
        CurrentStory.ChoosePathString(startKnot);

        BindExternalFunctions();
    }

    // ============================
    // PLAYER ADVANCE (ÚNICO PUNTO)
    // ============================
    public void Advance()
    {
        if (CurrentStory == null)
            return;

        if (CurrentStory.canContinue)
        {
            string line = CurrentStory.Continue();
            string speaker = GetSpeakerFromTags();

            if (!string.IsNullOrWhiteSpace(line))
            {
                OnLine?.Invoke(line.Trim(), speaker);
            }
            return;
        }

        // No hay más texto → cerrar
        EndStory();
    }

    private string GetSpeakerFromTags()
    {
        foreach (var tag in CurrentStory.currentTags)
        {
            if (tag.StartsWith("speaker:"))
                return tag.Replace("speaker:", "").Trim();
        }
        return null;
    }

    private void EndStory()
    {
        CurrentStory = null;
        OnDialogueEnd?.Invoke();
    }

    public bool IsRunning => CurrentStory != null;
}