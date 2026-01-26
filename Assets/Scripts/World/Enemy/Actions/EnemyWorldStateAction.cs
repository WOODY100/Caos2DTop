using UnityEngine;

public class EnemyWorldStateAction : MonoBehaviour
{
    [Header("Optional WorldState Action")]
    [SerializeField] private string flagOnDeath;
    private bool executed;

    public void ExecuteOnDeath()
    {
        if (executed)
            return;

        if (!string.IsNullOrEmpty(flagOnDeath))
        {
            WorldStateManager.Instance.SetFlag(flagOnDeath);
            executed = true;
        }
    }
}
