using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        panel.SetActive(true); // 🔥 PRIMERO mostrar
        GamePauseManager.Instance.RequestPause(this); // 🔥 LUEGO pausar

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

    private void Close()
    {
        panel.SetActive(false); // 🔥 primero ocultar
        GamePauseManager.Instance.ReleasePause(this); // 🔥 luego reanudar
    }

    private void ClearButtons()
    {
        foreach (Transform child in choicesParent)
        {
            Destroy(child.gameObject);
        }
    }
}
