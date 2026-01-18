using UnityEngine;
using TMPro;
using System.Collections;

public class FeedbackPopupUI : MonoBehaviour
{
    public static FeedbackPopupUI Instance;

    [SerializeField] private TMP_Text messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 1.5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.2f, 0);

    Coroutine routine;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        canvasGroup.alpha = 0f;
    }

    public void Show(string message, Vector3 worldPos)
    {
        messageText.text = message;
        transform.position = worldPos + offset;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(duration);
        canvasGroup.alpha = 0f;
        routine = null;
    }
}
