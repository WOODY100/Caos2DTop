using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteractDetector : MonoBehaviour
{
    private PlayerInteractor interactor;

    void Awake()
    {
        interactor = GetComponentInParent<PlayerInteractor>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
            interactor.SetInteractable(interactable);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();
        if (interactable != null)
        {
            interactor.ClearInteractable(interactable);
        }
    }
}
