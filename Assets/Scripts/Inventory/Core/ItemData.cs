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

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;

    public bool stackable;
    public int maxStack = 99;

    [Header("Stats")]
    public int bonusHealth;
    public int bonusAttack;
    public int bonusDefense;
    public float bonusSpeed;

}


