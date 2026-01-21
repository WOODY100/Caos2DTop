using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HealthBarUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private float fillSpeed = 5f;

    private PlayerStats playerStats;
    private Coroutine fillCoroutine;

    private void Awake()
    {
        playerStats = Object.FindFirstObjectByType<PlayerStats>();
    }

    private void OnEnable()
    {
        if (playerStats != null)
            playerStats.OnHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void OnDisable()
    {
        if (playerStats != null)
            playerStats.OnHealthChanged -= UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        if (playerStats == null) return;

        float targetFill = (float)playerStats.currentHealth / playerStats.maxHealth;

        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);

        fillCoroutine = StartCoroutine(AnimateFill(targetFill));

        hpText.text = $"{playerStats.currentHealth} / {playerStats.maxHealth}";
    }

    private IEnumerator AnimateFill(float target)
    {
        while (!Mathf.Approximately(fillImage.fillAmount, target))
        {
            fillImage.fillAmount = Mathf.MoveTowards(
                fillImage.fillAmount,
                target,
                fillSpeed * Time.unscaledDeltaTime
            );
            yield return null;
        }
    }
}
