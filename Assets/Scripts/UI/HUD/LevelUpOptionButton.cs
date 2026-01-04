using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class LevelUpOptionButton : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public Button button;

    private LevelUpOption option;
    private Action<LevelUpOption> onSelected;

    public void Setup(LevelUpOption option, System.Action<LevelUpOption> onClick)
    {
        this.option = option;

        titleText.text = option.title;
        descriptionText.text =
            $"{option.description}\n" +
            $"+HP {option.health}  " +
            $"+ATK {option.attack}  " +
            $"+DEF {option.defense}  " +
            $"+SPD {option.speed}";

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClick(option));
    }

    void Select()
    {
        onSelected?.Invoke(option);
    }
}
