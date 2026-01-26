using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Items/Health")]
public class HealthPotionItem : ItemData, IUsableItem
{
    public int healAmount;
    public float cooldown = 2f;

    public float CooldownDuration => cooldown;

    public bool CanUse(PlayerStats target)
    {
        if (target == null)
            return false;

        if (target.currentHealth >= target.maxHealth)
            return false;

        return !ItemCooldownManager.Instance.IsOnCooldown(this);
    }

    public bool Use(PlayerStats target)
    {
        if (!CanUse(target))
            return false;

        target.Heal(healAmount);
        ItemCooldownManager.Instance.StartCooldown(this);
        return true;
    }
}
