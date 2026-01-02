using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryDebugTester : MonoBehaviour
{
    [Header("Test Items")]
    public ItemData sword;
    public ItemData helmet;
    public ItemData necklace;
    public ItemData ring;
    public ItemData coin;

    private void Start()
    {
        InventoryManager.Instance.AddItem(sword, 1);
        InventoryManager.Instance.AddItem(helmet, 1);
    }

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            InventoryManager.Instance.AddItem(sword, 1);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            InventoryManager.Instance.AddItem(helmet, 1);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            InventoryManager.Instance.AddItem(necklace, 1);

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
            InventoryManager.Instance.AddItem(ring, 1);

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
            InventoryManager.Instance.AddItem(coin, 1);

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Debug.Log("Inventario limpiado");
            InventoryManager.Instance.items.Clear();
        }
    }
}
