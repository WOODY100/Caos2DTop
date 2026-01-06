using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TransitionTrigger : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,
        LoadScene
    }

    [Header("Transition")]
    public TransitionType transitionType;

    [Header("Same Scene")]
    public Transform teleportTarget;

    [Header("Load Scene")]
    public string sceneName;
    public string spawnID;

    [Header("Interaction")]
    public bool requireInput = true;
    public InputActionReference interactAction;

    private bool playerInside;
    private bool isTransitioning;

    private void OnEnable()
    {
        if (interactAction != null)
            interactAction.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        if (interactAction != null)
            interactAction.action.performed -= OnInteract;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        if (!requireInput)
        {
            ExecuteTransition();
        }
        else
        {
            interactAction?.action.Enable();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        interactAction?.action.Disable();
    }

    void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!playerInside || !requireInput) return;
        ExecuteTransition();
    }

    void ExecuteTransition()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (transitionType == TransitionType.SameScene)
        {
            player.transform.position = teleportTarget.position;
        }
        else
        {
            SpawnManager.Instance.SetSpawn(spawnID);
            SceneManager.LoadScene(sceneName);
        }
    }
}
