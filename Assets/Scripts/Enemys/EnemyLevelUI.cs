using TMPro;
using UnityEngine;

public class EnemyLevelUI : MonoBehaviour
{
    [Header("References")]
    public TMP_Text levelText;
    public EnemyLevel enemyLevel;

    [Header("Colors")]
    public Color lowLevelColor = Color.green;
    public Color equalLevelColor = Color.yellow;
    public Color highLevelColor = Color.red;

    private PlayerExperience player;

    private void Awake()
    {
        if (enemyLevel == null)
            enemyLevel = GetComponentInParent<EnemyLevel>();

        player = FindAnyObjectByType<PlayerExperience>();
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (enemyLevel == null || player == null) return;

        int enemyLv = enemyLevel.level;
        int playerLv = player.level;

        levelText.text = $"Lv. {enemyLv}";

        int diff = enemyLv - playerLv;

        if (diff <= -2)
            levelText.color = lowLevelColor;      // fácil
        else if (diff <= 1)
            levelText.color = equalLevelColor;    // normal
        else
            levelText.color = highLevelColor;     // peligro
    }

    private void OnEnable()
    {
        PlayerExperience exp = FindAnyObjectByType<PlayerExperience>();
        if (exp != null)
            exp.OnLevelUp += Refresh;
    }

    private void OnDisable()
    {
        PlayerExperience exp = FindAnyObjectByType<PlayerExperience>();
        if (exp != null)
            exp.OnLevelUp -= Refresh;
    }

}
