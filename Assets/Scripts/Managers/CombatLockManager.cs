using System;
using UnityEngine;

public class CombatLockManager : MonoBehaviour
{
    public static CombatLockManager Instance;

    [Header("Combat Settings")]
    [SerializeField] private float combatCooldown = 10f;

    [SerializeField] private float combatTimer = 0f;

    private int enemiesNearby = 0;
    private bool damageRecently = false;
    private int hostileEnemies = 0;

    public bool IsInCombat => combatTimer > 0f;
    public float CombatNormalizedTime => Mathf.Clamp01(combatTimer / combatCooldown);

    public static event Action OnCombatStart;
    public static event Action OnCombatEnd;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (hostileEnemies > 0 || damageRecently)
        {
            combatTimer = combatCooldown;
            damageRecently = false;
            return;
        }

        if (combatTimer > 0f)
        {
            combatTimer -= Time.deltaTime;

            if (combatTimer <= 0f)
            {
                combatTimer = 0f;
                OnCombatEnd?.Invoke();
            }
        }
    }

    // -------------------------
    // ENEMY SIGNALS
    // -------------------------

    public void RegisterHostileEnemy()
    {
        bool wasInCombat = IsInCombat;

        hostileEnemies++;

        if (!wasInCombat)
            OnCombatStart?.Invoke();
    }

    public void UnregisterHostileEnemy()
    {
        hostileEnemies = Mathf.Max(0, hostileEnemies - 1);
    }

    public void RegisterEnemy()
    {
        bool wasInCombat = IsInCombat;
        enemiesNearby++;

        if (!wasInCombat)
            OnCombatStart?.Invoke();
    }

    public void UnregisterEnemy()
    {
        enemiesNearby = Mathf.Max(0, enemiesNearby - 1);
    }

    // -------------------------
    // DAMAGE SIGNAL
    // -------------------------

    public void NotifyDamage()
    {
        bool wasInCombat = IsInCombat;

        damageRecently = true;
        combatTimer = combatCooldown;

        if (!wasInCombat)
            OnCombatStart?.Invoke();
    }
}
