using UnityEngine;

public class PotionCooldownManager : MonoBehaviour
{
    [SerializeField] private float potionCooldown = 2f;

    private float lastUseTime = -999f;

    public bool CanUsePotion()
    {
        return Time.time >= lastUseTime + potionCooldown;
    }

    public void RegisterUse()
    {
        lastUseTime = Time.time;
    }

    public float GetRemainingCooldown()
    {
        float remaining = (lastUseTime + potionCooldown) - Time.time;
        return Mathf.Max(0f, remaining);
    }

    // ✅ ESTE MÉTODO FALTABA
    public float GetTotalCooldown()
    {
        return potionCooldown;
    }
}
