using UnityEngine;

[CreateAssetMenu(menuName = "Items/Consumables/Regen Potion")]
public class RegenPotionItem : ItemData, IUsableItem
{
    [Header("Regeneration")]
    public int totalHeal = 50;
    public float duration = 5f;

    public bool Use()
    {
        PlayerStats stats = PlayerStatsProvider.Get();
        if (stats == null) return false;

        if (stats.IsHealthFull())
            return false;

        PotionCooldownManager cooldown =
            stats.GetComponent<PotionCooldownManager>();

        if (cooldown != null && !cooldown.CanUsePotion())
            return false;

        HealthRegenEffect regen =
            stats.GetComponent<HealthRegenEffect>();

        if (regen == null)
            regen = stats.gameObject.AddComponent<HealthRegenEffect>();

        regen.StartRegen(totalHeal, duration);

        cooldown?.RegisterUse();
        return true;
    }
}
