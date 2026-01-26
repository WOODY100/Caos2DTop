using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public Controls Controls { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Controls = new Controls();
        Controls.Enable();
    }

    private void OnDisable()
    {
        Controls.Disable();
    }
}
