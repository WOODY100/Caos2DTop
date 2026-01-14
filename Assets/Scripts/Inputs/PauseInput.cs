using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInput : MonoBehaviour
{
    private PauseMenuUI pauseMenu;

    private void OnEnable()
    {
        InputManager.Instance.Controls.Player.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        InputManager.Instance.Controls.Player.Pause.performed -= OnPause;
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        if (GamePauseManager.Instance.IsPausedBy<InventoryHUD>())
            return;

        if (pauseMenu == null)
        {
            pauseMenu = FindFirstObjectByType<PauseMenuUI>(
                FindObjectsInactive.Include
            );

            if (pauseMenu == null)
                return;
        }

        pauseMenu.Toggle();
    }
}
