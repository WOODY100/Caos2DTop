using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SaveFeedbackUI : MonoBehaviour
{
    public static SaveFeedbackUI Instance;

    [Header("UI")]
    public TMP_Text feedbackText;
    public Image icon;

    [Header("Timing")]
    public float fadeDuration = 0.25f;
    public float visibleTime = 1.2f;

    [Header("Icon Animation")]
    public float rotateSpeed = 180f; // grados por segundo

    public float TotalDuration =>
    fadeDuration * 2f + visibleTime;

    private Coroutine iconRoutine;
    private Coroutine routine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SetAlpha(0f);
    }

    public void Show(string message)
    {
        feedbackText.text = message;

        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        // Fade In
        yield return Fade(0f, 1f);

        // Icon animation
        iconRoutine = StartCoroutine(AnimateIcon());

        // Visible
        yield return new WaitForSecondsRealtime(visibleTime);

        // Stop icon animation
        if (iconRoutine != null)
            StopCoroutine(iconRoutine);

        // Fade Out
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color textColor = feedbackText.color;
        Color iconColor = icon.color;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(from, to, t / fadeDuration);

            textColor.a = a;
            iconColor.a = a;

            feedbackText.color = textColor;
            icon.color = iconColor;

            yield return null;
        }

        textColor.a = to;
        iconColor.a = to;

        feedbackText.color = textColor;
        icon.color = iconColor;
    }

    private IEnumerator AnimateIcon()
    {
        float t = 0f;
        Vector3 baseScale = icon.transform.localScale;

        while (true)
        {
            t += Time.unscaledDeltaTime * 4f;
            float s = 1f + Mathf.Sin(t) * 0.1f;
            icon.transform.localScale = baseScale * s;
            yield return null;
        }
    }

    private void SetAlpha(float a)
    {
        Color tc = feedbackText.color;
        Color ic = icon.color;

        tc.a = a;
        ic.a = a;

        feedbackText.color = tc;
        icon.color = ic;
    }
}
