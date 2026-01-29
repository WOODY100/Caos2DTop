using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour, IPoolable
{
    [SerializeField] private GameObject prefabSource;
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private EnemyLevel enemyLevel;

    [HideInInspector] public int maxHealth;
    [Header("Health")]
    private int currentHealth;

    [Header("Death")]
    public float corpseDuration = 60f;
    public event Action<GameObject> OnEnemyDied;

    public bool IsDead { get; private set; }
    public int CurrentHealth { get => currentHealth; }

    private SpriteRenderer spriteRenderer;
    private EnemyController controller;
    private EnemyAnimator animator;
    private EnemyExperience exp;
    private EnemyLootDrop lootDrop;
    private EnemyAIBase ai;
    private EnemyAttack attack;
    private Rigidbody2D rb;
    private NavMeshAgent agent;
    private EnemyLevelUI levelUI;

    void Awake()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        controller = GetComponent<EnemyController>();
        animator = GetComponent<EnemyAnimator>();
        exp = GetComponent<EnemyExperience>();
        lootDrop = GetComponent<EnemyLootDrop>();
        ai = GetComponent<EnemyAIBase>();
        attack = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        levelUI = GetComponentInChildren<EnemyLevelUI>(true);
        enemyLevel = GetComponent<EnemyLevel>();
        healthBar?.Refresh(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount, bool isCritical = false)
    {
        if (IsDead) return;

        if (enemyLevel != null && enemyLevel.isBoss)
        {
            BossUIController.Instance?.ShowBoss(
                this,
                enemyLevel.HasBossName ? enemyLevel.BossName : "BOSS"
            );

            BossUIController.Instance?.UpdateHealth(currentHealth, maxHealth);
        }

        int finalDamage = Mathf.Max(1, amount);
        currentHealth -= finalDamage;
        healthBar?.Refresh(currentHealth, maxHealth);

        CombatLockManager.Instance?.NotifyDamage();

        FloatingTextManager.Instance.ShowDamage(
            finalDamage,
            transform.position + Vector3.up * 0.8f,
            isCritical
        );

        animator.PlayHurt();

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (IsDead) return;
        IsDead = true;

        OnEnemyDied?.Invoke(gameObject);

        GetComponent<EnemyWorldStateAction>()?.ExecuteOnDeath();
        GetComponent<EnemyWorldState>()?.MarkAsDead();

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 3;
        }
        
        // 🧾 UI de nivel / vida
        levelUI?.Hide();
        healthBar?.Hide();
        if (enemyLevel != null && enemyLevel.isBoss)
        {
            BossUIController.Instance?.Hide();
        }


        // 🎞️ Animación
        animator.PlayDeath();

        // 🎯 Recompensas
        exp?.GiveExperience();
        FloatingTextManager.Instance.ShowExp(
            exp.expReward,
            transform.position + Vector3.up * 1.2f
        );

        // 💰 Loot (prefab independiente)
        lootDrop?.DropLoot();

        // 🧠 IA / Ataque
        if (ai) ai.enabled = false;
        if (attack) attack.enabled = false;

        // 🧭 NavMesh
        if (agent)
        {
            agent.ResetPath();
            agent.enabled = false;
        }

        // 🧍 Física
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // 🚫 Controller
        if (controller)
        {
            controller.Stop();
            controller.enabled = false;
        }

        CombatLockManager.Instance?.UnregisterHostileEnemy();

        // ⏳ Cadáver
        StartCoroutine(CorpseRoutine());
        StartCoroutine(ReleaseAfterCorpse());

    }

    private IEnumerator ReleaseAfterCorpse()
    {
        yield return new WaitForSeconds(corpseDuration);

        EnemyPoolManager.Instance.ReleaseEnemy(gameObject, prefabSource);
    }

    IEnumerator CorpseRoutine()
    {
        yield return new WaitForSeconds(corpseDuration);
    }

    public void SetMaxHealth(int value)
    {
        maxHealth = value;
        currentHealth = maxHealth;
    }

    public void OnSpawned()
    {
        ResetEnemy();
    }

    private void ResetEnemy()
    {
        IsDead = false;
        currentHealth = maxHealth;

        animator.ResetToIdle();   // ← AQUÍ

        controller.enabled = true;
        controller.SetCanMove(true);

        if (ai) ai.enabled = true;
        if (attack) attack.enabled = true;

        if (agent)
        {
            agent.enabled = true;
            agent.ResetPath();
        }

        if (rb)
        {
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero;
        }

        levelUI?.Refresh();
        healthBar?.Refresh(currentHealth, maxHealth);
    }

    public void ResetHealthOnly()
    {
        if (IsDead) return;

        currentHealth = maxHealth;

        // UI
        levelUI?.Refresh();
        healthBar?.Refresh(currentHealth, maxHealth);
        if (enemyLevel != null && enemyLevel.isBoss)
        {
            BossUIController.Instance?.Hide();
        }


        // Animación: volver a idle limpio
        animator.ResetToIdle();
    }
}
