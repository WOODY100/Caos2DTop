using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] private InventoryHUD inventoryHUD;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();

        controls.UI.ToggleInventory.performed += _ => inventoryHUD.Toggle();
        controls.UI.Close.performed += _ => inventoryHUD.Close();
    }

    private void OnEnable()
    {
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        controls.UI.Disable();
    }
}
