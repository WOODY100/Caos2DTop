using UnityEngine;

public class ChestLootContainer : LootContainer
{
    protected override void OnLootEmpty()
    {
        LootUI.Instance?.Close();

        // ❌ NO destruir
        // ❌ NO desactivar visualRoot
        // El cofre se maneja desde el script Chest
    }
}
