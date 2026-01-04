using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelUpChoiceButton : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descText;
    public Button button;

    private LevelUpOption option;

    public void Setup(LevelUpOption data, Action<LevelUpOption> onClick)
    {
        option = data;
        titleText.text = data.title;
        descText.text = data.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick(option));
    }
}
