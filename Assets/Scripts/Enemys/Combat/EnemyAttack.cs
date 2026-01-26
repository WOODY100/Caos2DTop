using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Attack")]
    public float attackCooldown = 1.2f;
    public bool IsAttacking => isAttacking;
    private float attackTimer;
    private EnemyAIBase enemyAI;
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
        enemyAI = GetComponentInParent<EnemyAIBase>();

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

        switch (enemyController.Facing)
        {
            case EnemyController.FacingDirection.Up:
                currentHitbox = hitboxUp;
                break;

            case EnemyController.FacingDirection.Down:
                currentHitbox = hitboxDown;
                break;

            case EnemyController.FacingDirection.Left:
                currentHitbox = hitboxLeft;
                break;

            case EnemyController.FacingDirection.Right:
                currentHitbox = hitboxRight;
                break;
        }
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
        enemyController.LockFacing();

        SelectHitbox();
        enemyAnimator.PlayAttack();
    }

    void EnterHit()
    {
        // 🔴 Cancelar ataque si el target ya no es válido
        if (!CanAttackTarget())
        {
            CancelAttack();
            return;
        }

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
        enemyController.SetCanMove(true);
        enemyController.UnlockFacing();

        // 🔥 CLAVE ABSOLUTA
        enemyAI?.OnAttackFinished();
    }

    private bool CanAttackTarget()
    {
        if (enemyAI == null)
            return true;

        float distance = Vector2.Distance(
            transform.position,
            enemyAI.Player.position
        );

        return distance <= enemyAI.attackDistance;
    }

    void CancelAttack()
    {
        currentState = AttackState.Idle;
        isAttacking = false;
        canAttack = true;

        DisableAllHitboxes();
        enemyController.SetCanMove(true);
        enemyController.UnlockFacing();

        // 🔔 AVISAR A LA IA (MISMO QUE FIN NORMAL)
        enemyAI?.OnAttackFinished();
    }
}
