using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemTooltipUI : MonoBehaviour
{
    public static ItemTooltipUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private Image iconImage;      // 🔹 FALTA ESTO
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private TMP_Text descriptionText;

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

        // 🔹 ICONO (CLAVE)
        if (iconImage != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = item.icon != null;
            iconImage.color = Color.white;
        }

        nameText.text = item.itemName;
        statsText.text = item.GetTooltipText();

        // DESCRIPCIÓN
        if (descriptionText != null)
        {
            descriptionText.text = string.IsNullOrEmpty(item.description)
                ? ""
                : item.description;

            descriptionText.gameObject.SetActive(!string.IsNullOrEmpty(item.description));
        }


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
