using UnityEngine;
using TMPro;

public class LevelUpUI : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text levelText;
    public TMP_Text statsText;

    private PlayerExperience playerExp;

    private void Awake()
    {
        playerExp = FindFirstObjectByType<PlayerExperience>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void Show(LevelStats gainedStats, int newLevel)
    {
        levelText.text = $"Nivel {newLevel}";

        if (gainedStats != null)
        {
            statsText.text =
                $"+ Vida: {gainedStats.bonusHealth}\n" +
                $"+ Ataque: {gainedStats.bonusAttack}\n" +
                $"+ Defensa: {gainedStats.bonusDefense}\n" +
                $"+ Velocidad: {gainedStats.bonusSpeed}";
        }
        else
        {
            statsText.text = "Sin mejoras en este nivel";
        }

        gameObject.SetActive(true);
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }
}
