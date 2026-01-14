using UnityEngine;

public class SaveListDebug : MonoBehaviour
{
    private void Start()
    {
        var saves = SaveManager.Instance.GetAllSaves();

        foreach (var save in saves)
        {
            Debug.Log(
                $"Slot {save.slot} | " +
                $"{save.meta.saveName} | " +
                $"Nivel {save.meta.playerLevel} | " +
                $"{save.meta.sceneName} | " +
                $"{save.meta.lastPlayed}"
            );
        }
    }
}
