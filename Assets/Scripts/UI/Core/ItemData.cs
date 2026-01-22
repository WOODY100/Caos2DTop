using UnityEngine;


public enum ItemType
{
    Consumible,
    Moneda,
    Llave,
    Material,

    Helmet,
    Armor,
    Legs,
    Boots,
    Sword,
    Necklace,
    Ring
}

[CreateAssetMenu(menuName = "Inventory/Items/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;

    [Header("Description")]
    [TextArea(3, 6)]
    public string description;

    public bool stackable;
    public int maxStack = 99;

    [Header("Stats")]
    public int bonusHealth;
    public int bonusAttack;
    public int bonusDefense;
    public float bonusSpeed;

    [Header("Save")]
    public string itemID;

    public int GetStatScore()
    {
        int score = 0;

        score += bonusHealth;
        score += bonusAttack * 2;   // pondera ataque
        score += bonusDefense * 2;
        score += Mathf.RoundToInt(bonusSpeed * 10f);

        return score;
    }

    public string GetTooltipText()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        if (bonusHealth != 0)
            sb.AppendLine($"Vida: {bonusHealth}");

        if (bonusAttack != 0)
            sb.AppendLine($"Ataque: {bonusAttack}");

        if (bonusDefense != 0)
            sb.AppendLine($"Defensa: {bonusDefense}");

        if (bonusSpeed != 0)
            sb.AppendLine($"Velocidad: {bonusSpeed}");

        return sb.ToString();
    }

}