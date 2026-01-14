using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour
{
    private InventoryHUD inventoryHUD;

    private void OnEnable()
    {
        InputManager.Instance.Controls.Player.Inventory.performed += OnInventory;
    }

    private void OnDisable()
    {
        InputManager.Instance.Controls.Player.Inventory.performed -= OnInventory;
    }

    private void OnInventory(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        // 🚫 No permitir inventario en estados inválidos
        var state = GameStateManager.Instance.CurrentState;
        if (state == GameState.Transition ||
            state == GameState.LevelUp ||
            state == GameState.Dialogue)
            return;

        if (inventoryHUD == null)
        {
            inventoryHUD = FindFirstObjectByType<InventoryHUD>(
                FindObjectsInactive.Include
            );

            if (inventoryHUD == null)
                return;
        }

        inventoryHUD.Toggle();
    }

}
