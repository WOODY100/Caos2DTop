using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("ID único que conecta entrada y salida")]
    public string spawnID;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}
