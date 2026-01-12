using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ItemTooltipUI : MonoBehaviour
{
    public static ItemTooltipUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text statsText;

    void Awake()
    {
        Instance = this;

        if (root == null)
        {
            Debug.LogError("ItemTooltipUI: Root NO asignado en el Inspector");
            return;
        }

        Hide();
    }

    void Update()
    {
        if (root.activeSelf)
            FollowMouse();
    }

    public void Show(ItemData item)
    {
        if (item == null)
            return;

        nameText.text = item.itemName;
        statsText.text = item.GetTooltipText();

        root.SetActive(true);
    }

    public void Hide()
    {
        if (root != null)
            root.SetActive(false);
    }

    private void FollowMouse()
    {
        if (Mouse.current == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        root.transform.position = mousePos;
    }
}
