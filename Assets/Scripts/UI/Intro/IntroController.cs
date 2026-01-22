using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;

public class IntroController : MonoBehaviour
{
    [Header("Text Scroll")]
    [SerializeField] private RectTransform textTransform;
    [SerializeField] private float scrollSpeed = 30f;
    [SerializeField] private float endY = 800f;

    [Header("Skip Settings")]
    [SerializeField] private float holdToSkipTime = 1.2f;
    [SerializeField] private Image skipFillImage;
    [SerializeField] private CanvasGroup skipCanvasGroup;

    private float holdTimer;
    private bool skipTriggered;
    private bool skipEnabled;

    private Coroutine introRoutine;

    private void Start()
    {
        int slot = SaveManager.Instance.CurrentSlot;

        bool globalIntroSeen = PlayerPrefs.GetInt("GLOBAL_INTRO_SEEN", 0) == 1;

        skipEnabled =
            globalIntroSeen ||
            SaveManager.Instance.HasSeenIntro(slot);

        if (!skipEnabled && skipCanvasGroup != null)
            skipCanvasGroup.gameObject.SetActive(false);

        FadeManager.Instance.SetAlpha(1f);

        if (skipFillImage != null)
            skipFillImage.fillAmount = 0f;

        if (skipCanvasGroup != null)
            skipCanvasGroup.alpha = 0f;

        introRoutine = StartCoroutine(IntroSequence());

        if (skipEnabled && skipCanvasGroup != null)
            StartCoroutine(ShowSkipDelayed());
    }


    private void Update()
    {
        if (!skipEnabled || skipTriggered)
            return;

        if (Keyboard.current.spaceKey.isPressed)
        {
            holdTimer += Time.unscaledDeltaTime;
            skipFillImage.fillAmount = holdTimer / holdToSkipTime;

            if (holdTimer >= holdToSkipTime)
                TriggerSkip();
        }
        else
        {
            holdTimer = Mathf.Max(holdTimer - Time.unscaledDeltaTime * 2f, 0f);
            skipFillImage.fillAmount = holdTimer / holdToSkipTime;
        }
    }

    private IEnumerator IntroSequence()
    {
        yield return FadeManager.Instance.FadeIn();
        yield return ScrollText();
        EndIntro();
    }

    private IEnumerator ScrollText()
    {
        Vector2 pos = textTransform.anchoredPosition;

        while (pos.y < endY && !skipTriggered)
        {
            pos.y += scrollSpeed * Time.unscaledDeltaTime;
            textTransform.anchoredPosition = pos;
            yield return null;
        }
    }

    private void TriggerSkip()
    {
        skipTriggered = true;

        if (introRoutine != null)
            StopCoroutine(introRoutine);

        EndIntro();
    }

    private void EndIntro()
    {
        int slot = SaveManager.Instance.CurrentSlot;

        SaveManager.Instance.MarkIntroSeen(slot);

        PlayerPrefs.SetInt("GLOBAL_INTRO_SEEN", 1);
        PlayerPrefs.Save();

        StartCoroutine(LoadGameAfterFade());
    }

    private IEnumerator LoadGameAfterFade()
    {
        yield return FadeManager.Instance.FadeOut();
        SaveManager.Instance.LoadGame(SaveManager.Instance.CurrentSlot);
    }

    private IEnumerator ShowSkipDelayed()
    {
        yield return new WaitForSecondsRealtime(2f);

        float t = 0f;
        while (t < 0.5f)
        {
            t += Time.unscaledDeltaTime;
            skipCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / 0.5f);
            yield return null;
        }
    }
}
