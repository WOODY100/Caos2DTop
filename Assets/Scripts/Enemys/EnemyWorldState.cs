using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWorldState : MonoBehaviour, ISaveable
{
    [SerializeField] private string enemyID;

    private void Awake()
    {
        if (string.IsNullOrEmpty(enemyID))
            GenerateID();
    }

    public void LoadData(SaveData data)
    {
        if (WorldStateManager.Instance == null)
            return;

        if (WorldStateManager.Instance.IsEnemyDead(enemyID))
        {
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

    // =========================
    // ID AUTOMÁTICO
    // =========================
    private void GenerateID()
    {
        string scene = SceneManager.GetActiveScene().name;
        Vector3 pos = transform.position;

        enemyID = $"{scene}_Enemy_{pos.x:F2}_{pos.y:F2}";
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.Label(transform.position + Vector3.up, enemyID);
    }
#endif
}
