using UnityEngine;

[CreateAssetMenu(menuName = "Items/Potion/Health")]
public class HealthPotionItem : ItemData, IUsableItem
{
    public int healAmount;

    public bool Use()
    {
        PlayerStats player = PlayerStats.Instance;

        if (player == null) return false;
        if (player.currentHealth >= player.maxHealth)
            return false;

        player.Heal(healAmount);
        return true;
    }
}
