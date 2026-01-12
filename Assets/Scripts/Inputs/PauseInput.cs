using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private PauseMenuUI pauseMenu;

    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        controls.Player.Pause.performed -= OnPause;
        controls.Disable();
    }

    void OnPause(InputAction.CallbackContext ctx)
    {
        if (GamePauseManager.Instance.IsPausedBy<InventoryHUD>())
            return;
        
        if (GamePauseManager.Instance.IsPausedBy<InventoryHUD>())
            return;

        if (pauseMenu == null)
        {
            Debug.LogError("PauseMenuUI no asignado en PauseInput");
            return;
        }

        pauseMenu.Toggle();
    }
}
