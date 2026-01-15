using UnityEngine;

public class EnemyWorldState : MonoBehaviour, ISaveable
{
    [SerializeField] private string enemyID;

    // ❌ NO usar Awake para WorldState

    public void LoadData(SaveData data)
    {
        if (WorldStateManager.Instance == null)
            return;

        if (WorldStateManager.Instance.IsEnemyDead(enemyID))
        {
            // 🔹 Ya murió en esta partida
            Destroy(gameObject);
        }
    }

    public void SaveData(SaveData data)
    {
        // ❌ No guardamos nada aquí
        // WorldStateManager ya lo hace
    }

    public void MarkAsDead()
    {
        WorldStateManager.Instance?.MarkEnemyDead(enemyID);
    }
}
