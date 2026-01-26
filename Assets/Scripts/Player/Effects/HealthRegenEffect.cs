using System.Collections;
using UnityEngine;

public class HealthRegenEffect : MonoBehaviour
{
    private PlayerStats playerStats;
    private Coroutine regenRoutine;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();

        if (playerStats == null)
        {
            Debug.LogError(
                "[HealthRegenEffect] No se encontró PlayerStats en el GameObject",
                this
            );
        }
    }

    public void StartRegen(int totalHeal, float duration)
    {
        if (playerStats == null) return;

        // Si ya había regeneración activa, la reiniciamos
        if (regenRoutine != null)
            StopCoroutine(regenRoutine);

        regenRoutine = StartCoroutine(RegenRoutine(totalHeal, duration));
    }

    private IEnumerator RegenRoutine(int totalHeal, float duration)
    {
        float elapsed = 0f;
        float healPerSecond = totalHeal / duration;

        while (elapsed < duration)
        {
            playerStats.Heal(Mathf.CeilToInt(healPerSecond));
            elapsed += 1f;
            yield return new WaitForSeconds(1f);
        }

        regenRoutine = null;
    }

    public void StopRegen()
    {
        if (regenRoutine != null)
        {
            StopCoroutine(regenRoutine);
            regenRoutine = null;
        }
    }
}
