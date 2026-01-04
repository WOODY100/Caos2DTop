using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBarUI : MonoBehaviour
{
    [Header("UI")]
    public Image fillImage;
    public TMP_Text levelText;
    public TMP_Text expText;

    [Header("Animation")]
    public float fillSpeed = 5f;

    private PlayerExperience playerExp;
    private float targetFill;

    private void Start()
    {
        playerExp = FindFirstObjectByType<PlayerExperience>();

        playerExp.OnExpChanged += UpdateUI;
        playerExp.OnLevelUp += OnLevelUp;

        UpdateUI();
    }

    private void Update()
    {
        fillImage.fillAmount = Mathf.Lerp(
            fillImage.fillAmount,
            targetFill,
            Time.deltaTime * fillSpeed
        );
    }

    private void OnLevelUp(LevelStats stats, int level)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        targetFill = (float)playerExp.currentExp / playerExp.expToNextLevel;
        levelText.text = "Lv " + playerExp.level;
        expText.text = $"{playerExp.currentExp} / {playerExp.expToNextLevel}";
    }
}
