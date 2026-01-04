using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class LevelUpUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public TMP_Text titleText;
    public Transform choicesParent;
    public LevelUpOptionButton optionButtonPrefab;

    private Action<LevelUpOption> onOptionSelected;

    private void Awake()
    {
        panel.SetActive(false);
    }

    public void Show(List<LevelUpOption> options, Action<LevelUpOption> callback)
    {
        onOptionSelected = callback;

        panel.SetActive(true);
        GamePauseManager.Instance.RequestPause(this);

        ClearButtons();

        foreach (var opt in options)
        {
            LevelUpOptionButton btn =
                Instantiate(optionButtonPrefab, choicesParent);

            btn.Setup(opt, OnOptionSelected);
        }
    }

    private void OnOptionSelected(LevelUpOption option)
    {
        onOptionSelected?.Invoke(option);
        Close();
    }

    private void ClearButtons()
    {
        foreach (Transform child in choicesParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void Close()
    {
        GamePauseManager.Instance.ReleasePause(this);
        panel.SetActive(false);
    }
}
