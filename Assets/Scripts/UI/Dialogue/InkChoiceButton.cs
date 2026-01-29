using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InkChoiceButton : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Button button;

    private int choiceIndex;

    public void Setup(string text, int index)
    {
        label.text = text;
        choiceIndex = index;
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        //InkManager.Instance.Choose(choiceIndex);
    }
}