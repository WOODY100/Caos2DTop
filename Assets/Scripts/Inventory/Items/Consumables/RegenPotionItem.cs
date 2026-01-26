using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Items/Regen Potion")]
public class RegenPotionItem : ItemData, IUsableItem
{
    [Header("Regeneration")]
    public int totalHeal = 50;
    public float duration = 5f;

    [Header("Cooldown")]
    public float cooldown = 8f;

    // ✅ IMPLEMENTACIÓN OBLIGATORIA
    public float CooldownDuration => cooldown;

    public bool CanUse(PlayerStats target)
    {
        if (target == null)
            return false;

        if (target.IsHealthFull())
            return false;

        return !ItemCooldownManager.Instance.IsOnCooldown(this);
    }

    public bool Use(PlayerStats target)
    {
        if (!CanUse(target))
            return false;

        HealthRegenEffect regen =
            target.GetComponent<HealthRegenEffect>() ??
            target.gameObject.AddComponent<HealthRegenEffect>();

        regen.StartRegen(totalHeal, duration);

        ItemCooldownManager.Instance.StartCooldown(this);
        return true;
    }
}
