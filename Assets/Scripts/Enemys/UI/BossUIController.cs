using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUIController : MonoBehaviour
{
    public static BossUIController Instance;

    [Header("UI")]
    [SerializeField] private TMP_Text bossNameText;
    [SerializeField] private Image healthFill;
    [SerializeField] private CanvasGroup canvasGroup;

    private EnemyHealth currentBoss;
    private int maxHealth;

    void Awake()
    {
        Instance = this;
        HideInstant();
    }

    public void ShowBoss(EnemyHealth bossHealth, string bossName)
    {
        currentBoss = bossHealth;
        maxHealth = bossHealth.maxHealth;

        bossNameText.text = bossName;
        UpdateHealth(bossHealth.CurrentHealth, maxHealth);

        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
    }

    public void UpdateHealth(int current, int max)
    {
        if (healthFill == null) return;
        healthFill.fillAmount = Mathf.Clamp01((float)current / max);
    }

    public void Hide()
    {
        currentBoss = null;
        gameObject.SetActive(false);
    }

    void HideInstant()
    {
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
