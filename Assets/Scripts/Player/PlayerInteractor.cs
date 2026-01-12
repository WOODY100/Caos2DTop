using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    private IInteractable currentInteractable;

    // 🔔 Llamado por detectores (trigger, raycast, etc.)
    public void SetInteractable(IInteractable interactable)
    {
        if (interactable == null)
            return;

        currentInteractable = interactable;
    }

    public void ClearInteractable(IInteractable interactable)
    {
        if (currentInteractable == interactable)
            currentInteractable = null;
    }

    public void OnInteract()
    {
        TryInteract();
    }

    private void TryInteract()
    {
        if (currentInteractable == null)
            return;

        // 🔒 Blindaje: el objeto pudo haber sido destruido
        MonoBehaviour mb = currentInteractable as MonoBehaviour;
        if (mb == null || !mb.isActiveAndEnabled)
        {
            currentInteractable = null;
            return;
        }

        currentInteractable.Interact();
    }

    void OnDisable()
    {
        // Limpieza automática (cambio de escena, desactivado, etc.)
        currentInteractable = null;
    }

    public void OnCancel()
    {
        if (LootUI.Instance != null)
        {
            LootUI.Instance.Close();
        }
    }

}
