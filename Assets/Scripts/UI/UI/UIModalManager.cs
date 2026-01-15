using UnityEngine;

public class UIModalManager : MonoBehaviour
{
    public static UIModalManager Instance { get; private set; }

    private object currentModal;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    /// <summary>
    /// Intenta abrir una UI modal.
    /// Devuelve false si otra UI ya está abierta.
    /// </summary>
    public bool RequestOpen(object requester)
    {
        if (currentModal != null)
            return false;

        currentModal = requester;
        return true;
    }

    /// <summary>
    /// Cierra la UI modal solo si es la dueña actual.
    /// </summary>
    public void Close(object requester)
    {
        if (currentModal == requester)
            currentModal = null;
    }

    /// <summary>
    /// ¿Hay alguna UI modal abierta?
    /// </summary>
    public bool HasOpenModal()
    {
        return currentModal != null;
    }

    /// <summary>
    /// ¿La UI abierta es de cierto tipo?
    /// </summary>
    public bool IsOpenBy<T>()
    {
        return currentModal is T;
    }

    public void ResetModal()
    {
        currentModal = null;
    }
}
