using UnityEngine;

public class ChestLootContainer : LootContainer
{
    private void Start()
    {
        EnableLoot();
    }

    protected override void OnLootEmpty()
    {
        GetComponent<ChestWorldStateAction>()?.Execute();
        LootUI.Instance?.Close();
    }
}