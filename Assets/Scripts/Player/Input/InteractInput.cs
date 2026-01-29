using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInteractor))]
public class InteractInput : MonoBehaviour
{
    private PlayerInteractor interactor;

    private void Awake()
    {
        interactor = GetComponent<PlayerInteractor>();
    }

    private void OnEnable()
    {
        InputManager.Instance.Controls.Player.Interact.performed += OnInteract;
        InputManager.Instance.Controls.Player.Cancel.performed += OnCancel;
    }

    private void OnDisable()
    {
        InputManager.Instance.Controls.Player.Interact.performed -= OnInteract;
        InputManager.Instance.Controls.Player.Cancel.performed -= OnCancel;
    }

    private void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        /*if (GameStateManager.Instance != null &&
            !GameStateManager.Instance.IsGameplayAllowed())
            return;*/

        Debug.Log("[INPUT] Interact (E)");

        interactor.OnInteract();
    }

    private void OnCancel(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;

        interactor.OnCancel();
    }
}
