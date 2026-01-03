using TMPro;
using UnityEngine;
using System;

public class PlayerStatsUI : MonoBehaviour
{
    public TMP_Text attackText;
    public TMP_Text defenseText;
    public TMP_Text healthText;
    public TMP_Text speedText;

    private PlayerStats stats;

    private void Awake()
    {
        stats = FindAnyObjectByType<PlayerStats>();
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
    }
}
