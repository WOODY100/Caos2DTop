using TMPro;
using UnityEngine;
using System;

public class PlayerStatsUI : MonoBehaviour
{
    public TMP_Text attackText;
    public TMP_Text defenseText;
    public TMP_Text healthText;
    public TMP_Text speedText;
    public TMP_Text levelText;

    private PlayerStats stats;
    private PlayerExperience level;

    private void Awake()
    {
        stats = FindAnyObjectByType<PlayerStats>();
        level = FindAnyObjectByType<PlayerExperience>();
    }

    private void OnEnable()
    {
        PlayerStats.OnStatsChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        PlayerStats.OnStatsChanged -= Refresh;
    }

    public void Refresh()
    {
        if (stats == null) return;

        attackText.text = stats.attack.ToString();
        defenseText.text = stats.defense.ToString();
        healthText.text = stats.maxHealth.ToString();
        speedText.text = stats.speed.ToString("0.0");
        levelText.text = level.level.ToString();
    }
}
