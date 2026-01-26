using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;

    [Header("Behavior")]
    [SerializeField] private bool hideWhenFull = true;
    [SerializeField] private float autoHideDelay = 2f;

    private EnemyHealth health;
    private Coroutine hideRoutine;

    void Awake()
    {
        health = GetComponentInParent<EnemyHealth>();

        if (fillImage != null)
            fillImage.fillAmount = 1f;
    }

    public void Refresh(int current, int max)
    {
        if (fillImage == null) return;

        float percent = Mathf.Clamp01((float)current / max);
        fillImage.fillAmount = percent;

        Show();

        if (hideWhenFull && percent >= 0.999f)
            AutoHide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
    }

    public void Hide()
    {
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }

        gameObject.SetActive(false);
    }

    void AutoHide()
    {
        if (autoHideDelay <= 0f) return;

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(autoHideDelay);
        gameObject.SetActive(false);
    }
}
