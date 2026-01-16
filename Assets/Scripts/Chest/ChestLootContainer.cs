using UnityEngine;

public class ChestLootContainer : LootContainer
{
    protected override void OnLootEmpty()
    {
        GetComponent<ChestWorldStateAction>()?.Execute();
        LootUI.Instance?.Close();

        // ❌ NO destruir
        // ❌ NO desactivar visualRoot
        // El cofre se maneja desde el script Chest
    }
}
