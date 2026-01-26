using UnityEngine;

public class PlayerPositionSave : MonoBehaviour, ISaveable
{
    public void SaveData(SaveData data)
    {
        data.playerPosition = transform.position;
    }

    public void LoadData(SaveData data)
    {
        // ❌ Si hay spawn activo, NO usar posición del save
        if (SpawnManager.Instance != null &&
            !string.IsNullOrEmpty(SpawnManager.Instance.nextSpawnID))
            return;

        transform.position = data.playerPosition;
    }
}
