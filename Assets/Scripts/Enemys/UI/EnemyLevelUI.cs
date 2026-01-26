using TMPro;
using UnityEngine;

public class EnemyLevelUI : MonoBehaviour
{
    [Header("Colors")]
    public Color lowLevelColor = Color.green;
    public Color equalLevelColor = Color.yellow;
    public Color highLevelColor = Color.red;

    private TMP_Text levelText;
    private EnemyLevel enemyLevel;
    private PlayerExperience player;

    private void Awake()
    {
        if (enemyLevel == null)
            enemyLevel = GetComponentInParent<EnemyLevel>();

        if (levelText == null)
            levelText = GetComponentInChildren<TMP_Text>(true);

        player = FindAnyObjectByType<PlayerExperience>();
    }


    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (levelText == null || enemyLevel == null || player == null)
            return;

        // 👑 BOSS CON NOMBRE
        if (enemyLevel.isBoss)
        {
            if (enemyLevel.HasBossName)
                levelText.text = enemyLevel.BossName;
            else
                levelText.text = "BOSS";

            levelText.color = highLevelColor;
            return;
        }

        // 🧍 Enemigos normales
        int enemyLv = enemyLevel.level;
        int playerLv = player.level;

        levelText.text = $"Lv {enemyLv}";

        int diff = enemyLv - playerLv;

        if (diff <= -2)
            levelText.color = lowLevelColor;
        else if (diff <= 1)
            levelText.color = equalLevelColor;
        else
            levelText.color = highLevelColor;
    }


    private void OnPlayerLevelUp(LevelStats stats, int level)
    {
        Refresh();
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (enemyLevel != null)
            enemyLevel.OnLevelChanged += Refresh;

        if (player != null)
            player.OnLevelUp += OnPlayerLevelUp;
    }

    private void OnDisable()
    {
        if (enemyLevel != null)
            enemyLevel.OnLevelChanged -= Refresh;

        if (player != null)
            player.OnLevelUp -= OnPlayerLevelUp;
    }
}
