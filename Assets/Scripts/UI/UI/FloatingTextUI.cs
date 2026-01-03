using TMPro;
using UnityEngine;

public class FloatingTextUI : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float lifetime = 1f;

    private TMP_Text text;
    private Color originalColor;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        originalColor = text.color;
    }

    public void Init(int value, Color color)
    {
        text.text = value.ToString();
        text.color = color;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += Vector3.up * (moveSpeed * 0.5f) * Time.deltaTime;

        // Fade
        text.color = Color.Lerp(
            text.color,
            new Color(text.color.r, text.color.g, text.color.b, 0),
            Time.deltaTime
        );
    }
    public void InitDamage(int value, bool isCritical)
    {
        text.text = value.ToString();
        text.color = isCritical ? Color.yellow : Color.white;
        text.fontSize = isCritical ? 6 : 4;

        Destroy(gameObject, lifetime);
    }


    public void InitExp(int value)
    {
        text.text = $"+{value} EXP";
        text.color = Color.cyan;
        Destroy(gameObject, lifetime);
    }

}
