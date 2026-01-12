using UnityEngine;

public class EnemyLootDrop : MonoBehaviour
{
    [Header("Loot")]
    [SerializeField] private LootContainer lootPrefab;

    public void DropLoot()
    {
        if (lootPrefab == null)
        {
            Debug.LogWarning($"{name} no tiene lootPrefab asignado");
            return;
        }

        LootContainer lootInstance = Instantiate(
            lootPrefab,
            transform.position,
            Quaternion.identity
        );

        // 🔥 ESTO ES LO QUE FALTABA
        lootInstance.EnableLoot();
    }
}
