using UnityEngine;

public class EnemyExperience : MonoBehaviour
{
    [Header("Experience Reward")]
    public int expReward = 10;

    public void GiveExperience()
    {
        PlayerExperience playerExp = FindAnyObjectByType<PlayerExperience>();

        if (playerExp == null)
        {
            Debug.LogWarning("PlayerExperience no encontrado");
            return;
        }

        playerExp.AddExp(expReward);
    }
}
