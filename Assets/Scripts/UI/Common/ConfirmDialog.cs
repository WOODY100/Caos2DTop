using UnityEngine;
using TMPro;
using System;

public class ConfirmDialog : MonoBehaviour
{
    public static ConfirmDialog Instance;

    public TMP_Text titleText;
    public TMP_Text messageText;

    private Action onConfirm;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameObject.SetActive(false); // oculto, pero Awake ya corrió
    }

    public void Show(string title, string message, Action confirmAction)
    {
        titleText.text = title;
        messageText.text = message;
        onConfirm = confirmAction;

        gameObject.SetActive(true);
    }

    public void OnYes()
    {
        onConfirm?.Invoke();
        Close();
    }

    public void OnNo()
    {
        Close();
    }

    private void Close()
    {
        onConfirm = null;
        gameObject.SetActive(false);
    }
}
