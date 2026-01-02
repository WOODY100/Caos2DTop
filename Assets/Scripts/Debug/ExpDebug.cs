using UnityEngine;
using UnityEngine.InputSystem;

public class ExpDebugInput : MonoBehaviour
{
    private Controls controls;
    private PlayerExperience playerExp;

    private void Awake()
    {
        controls = new Controls();
        playerExp = FindFirstObjectByType<PlayerExperience>();

        controls.Debug.AddExp.performed += ctx =>
        {
            playerExp.AddExp(25);
            Debug.Log("EXP +25");
        };
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
