using UnityEngine;
using UnityEngine.InputSystem;

public class HotbarInputHandler : MonoBehaviour
{
    private Controls input;
    private HotbarManager hotbar;

    private void Awake()
    {
        input = new Controls();
    }

    private void Start()
    {
        hotbar = HotbarManager.Instance;

        if (hotbar == null)
            Debug.LogError("[HotbarInputHandler] HotbarManager.Instance es null");
    }

    private void OnEnable()
    {
        input.Player.Enable();

        input.Player.Hotbar1.performed += _ => hotbar.TryUseSlot(0);
        input.Player.Hotbar2.performed += _ => hotbar.TryUseSlot(1);
        input.Player.Hotbar3.performed += _ => hotbar.TryUseSlot(2);
        input.Player.Hotbar4.performed += _ => hotbar.TryUseSlot(3);
        input.Player.Hotbar5.performed += _ => hotbar.TryUseSlot(4);
    }

    private void OnDisable()
    {
        input.Player.Hotbar1.performed -= _ => hotbar.TryUseSlot(0);
        input.Player.Hotbar2.performed -= _ => hotbar.TryUseSlot(1);
        input.Player.Hotbar3.performed -= _ => hotbar.TryUseSlot(2);
        input.Player.Hotbar4.performed -= _ => hotbar.TryUseSlot(3);
        input.Player.Hotbar5.performed -= _ => hotbar.TryUseSlot(4);

        input.Player.Disable();
    }
}
