using UnityEngine;
using UnityEngine.InputSystem;

public class SaveSlotDebugInput : MonoBehaviour
{
    private Controls controls;

    private void Awake()
    {
        controls = new Controls();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Save.performed += OnSavePerformed;
    }

    private void OnDisable()
    {
        controls.Player.Save.performed -= OnSavePerformed;
        controls.Player.Disable();
    }

    private void OnSavePerformed(InputAction.CallbackContext context)
    {
        // Detectar qué tecla fue presionada
        if (context.control == Keyboard.current.f5Key)
        {
            SaveManager.Instance.SaveGameToSlot(0);
            Debug.Log("Guardado en SLOT 0 (F5)");
        }
        else if (context.control == Keyboard.current.f6Key)
        {
            SaveManager.Instance.SaveGameToSlot(1);
            Debug.Log("Guardado en SLOT 1 (F6)");
        }
    }
}
