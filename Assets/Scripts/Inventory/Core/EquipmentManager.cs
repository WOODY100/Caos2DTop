using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance;
    private PlayerStats playerStats;


    [Header("Equipment Slots")]
    public EquipmentSlotUI helmet;
    public EquipmentSlotUI armor;
    public EquipmentSlotUI legs;
    public EquipmentSlotUI boots;
    public EquipmentSlotUI sword;
    public EquipmentSlotUI necklace;
    public EquipmentSlotUI ring;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        playerStats = FindAnyObjectByType<PlayerStats>();

    }

    public bool Equip(ItemData item)
    {
        if (item == null)
            return false;

        if (!IsEquipable(item))
        {
            return false;
        }

        switch (item.type)
        {
            case ItemType.Helmet:
                helmet.Equip(item);
                break;

            case ItemType.Armor:
                armor.Equip(item);
                break;

            case ItemType.Legs:
                legs.Equip(item);
                break;

            case ItemType.Boots:
                boots.Equip(item);
                break;

            case ItemType.Sword:
                sword.Equip(item);
                break;

            case ItemType.Necklace:
                necklace.Equip(item);
                break;

            case ItemType.Ring:
                ring.Equip(item);
                break;

            default:
                return false;
        }

        playerStats?.RecalculateStats();
        return true;
    }


    public void Unequip(ItemType type)
    {
        switch (type)
        {
            case ItemType.Helmet: helmet.Unequip(); break;
            case ItemType.Armor: armor.Unequip(); break;
            case ItemType.Legs: legs.Unequip(); break;
            case ItemType.Boots: boots.Unequip(); break;
            case ItemType.Sword: sword.Unequip(); break;
            case ItemType.Necklace: necklace.Unequip(); break;
            case ItemType.Ring: ring.Unequip(); break;
        }

        playerStats?.RecalculateStats();
    }

    private bool IsEquipable(ItemData item)
    {
        return item.type == ItemType.Helmet
            || item.type == ItemType.Armor
            || item.type == ItemType.Legs
            || item.type == ItemType.Boots
            || item.type == ItemType.Sword
            || item.type == ItemType.Necklace
            || item.type == ItemType.Ring;
    }

    public ItemData[] GetAllEquipped()
    {
        return new ItemData[]
        {
        helmet?.GetItem(),
        armor?.GetItem(),
        legs?.GetItem(),
        boots?.GetItem(),
        sword?.GetItem(),
        necklace?.GetItem(),
        ring?.GetItem()
        };
    }

}
