using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    private bool isOpen;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (isOpen)
            Close();
        else
            Open();
    }

    public void Open()
    {
        if (isOpen) return;

        isOpen = true;
        gameObject.SetActive(true);
        GamePauseManager.Instance.RequestPause(this);
    }

    public void Close()
    {
        if (!isOpen) return;

        isOpen = false;
        gameObject.SetActive(false);
        GamePauseManager.Instance.ReleasePause(this);
    }

    public void Resume()
    {
        Close();
    }
}
