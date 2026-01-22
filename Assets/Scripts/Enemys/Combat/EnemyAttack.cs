using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown = 1.2f;
    public bool IsAttacking => isAttacking;
    private float attackTimer;
    [SerializeField] private AttackState currentState = AttackState.Idle;

    [Header("Attack FSM Timers")]
    [SerializeField] private float windupTime = 0.15f;
    [SerializeField] private float hitTime = 0.25f;
    [SerializeField] private float recoveryTime = 0.2f;
    [SerializeField] private float cooldownTime = 0.8f;

    private float stateTimer;

    [Header("Hitboxes")]
    public GameObject hitboxUp;
    public GameObject hitboxDown;
    public GameObject hitboxLeft;
    public GameObject hitboxRight;

    private EnemyAnimator enemyAnimator;
    private EnemyController enemyController;
    private EnemyLevel enemyLevel;

    private bool canAttack = true;
    private bool isAttacking;
    private GameObject currentHitbox;

    public enum AttackState
    {
        Idle,
        Windup,
        Hit,
        Recovery,
        Cooldown
    }

    void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyController = GetComponent<EnemyController>();
        enemyLevel = GetComponent<EnemyLevel>();

        DisableAllHitboxes();
    }

    void Update()
    {
        if (currentState == AttackState.Idle)
            return;

        stateTimer -= Time.deltaTime;

        if (stateTimer > 0f)
            return;

        switch (currentState)
        {
            case AttackState.Windup:
                EnterHit();
                break;

            case AttackState.Hit:
                EnterRecovery();
                break;

            case AttackState.Recovery:
                EnterCooldown();
                break;

            case AttackState.Cooldown:
                ExitAttack();
                break;
        }
    }

    public int GetDamage()
    {
        return enemyLevel != null ? enemyLevel.attack : 1;
    }

    public void TryAttack()
    {
        if (currentState != AttackState.Idle) return;
        if (!canAttack) return;

        EnterWindup();
    }

    void ResetCooldown()
    {
        canAttack = true;
    }

    void SelectHitbox()
    {
        DisableAllHitboxes();

        Vector2 dir = enemyController.GetMoveDirection();
        if (dir == Vector2.zero)
            dir = Vector2.down;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            currentHitbox = dir.x > 0 ? hitboxRight : hitboxLeft;
        else
            currentHitbox = dir.y > 0 ? hitboxUp : hitboxDown;
    }
    
    void ForceEndAttack()
    {
        isAttacking = false;
        canAttack = true;
        enemyController.SetCanMove(true);
        DisableAllHitboxes();
    }

    // 🎞️ Animation Events (OFICIALES)
    public void Anim_AttackStart()
    {
        if (currentState == AttackState.Windup)
        {
            EnterHit();
        }
    }

    public void Anim_AttackEnd()
    {
        if (currentState == AttackState.Hit)
        {
            EnterRecovery();
        }
    }

    void DisableAllHitboxes()
    {
        hitboxUp.SetActive(false);
        hitboxDown.SetActive(false);
        hitboxLeft.SetActive(false);
        hitboxRight.SetActive(false);
    }

    // Metodos De entrada por estado
    void EnterWindup()
    {
        currentState = AttackState.Windup;
        stateTimer = windupTime;

        canAttack = false;
        isAttacking = true;

        enemyController.SetCanMove(false);
        enemyController.Stop();

        SelectHitbox();
        enemyAnimator.PlayAttack();
    }

    void EnterHit()
    {
        currentState = AttackState.Hit;
        stateTimer = hitTime;

        currentHitbox?.SetActive(true);
    }

    void EnterRecovery()
    {
        currentState = AttackState.Recovery;
        stateTimer = recoveryTime;

        DisableAllHitboxes();
    }

    void EnterCooldown()
    {
        currentState = AttackState.Cooldown;
        stateTimer = cooldownTime;

        enemyController.SetCanMove(true);
    }

    void ExitAttack()
    {
        currentState = AttackState.Idle;
        isAttacking = false;
        canAttack = true;

        DisableAllHitboxes();
    }

}
