using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyHealth))]
public abstract class EnemyAIBase : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Return,
        Attack
    }

    protected PlayerAttackSlots attackSlots;
    protected bool hasAttackSlot;

    [Header("Target")]
    [SerializeField] protected Transform player;

    [Header("Separation")]
    public float separationRadius = 0.6f;
    public float separationStrength = 1.2f;
    public LayerMask enemyLayer;

    [Header("Detection")]
    public float detectionDistance = 4f;
    public float chaseDistance = 7f;
    public float attackDistance = 1.1f;

    [Header("Idle")]
    public float idleTime = 1f;
    protected float idleTimer;

    [Header("Patrol")]
    public float patrolRadius = 3f;
    public float patrolWaitTime = 1.5f;
    protected float patrolTimer;

    protected State currentState;

    protected NavMeshAgent agent;
    protected EnemyController controller;
    protected EnemyHealth health;

    protected Vector3 originPosition;

    // ─────────────────────────────

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();
        health = GetComponent<EnemyHealth>();

        agent.obstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(30, 60);
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        originPosition = transform.position;
        idleTimer = idleTime;
        currentState = State.Idle;

        // 🔍 Auto-detectar Player
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        // 🎯 Attack slots (opcional)
        attackSlots = player
            ? player.GetComponent<PlayerAttackSlots>()
            : null;
    }


    protected virtual void Update()
    {
        // 🔴 CLAVE ABSOLUTA
        if (health != null && health.IsDead)
            return;

        float distanceToPlayer = player
            ? Vector2.Distance(transform.position, player.position)
            : Mathf.Infinity;

        switch (currentState)
        {
            case State.Idle:
                Idle();
                if (PlayerDetected()) ChangeState(State.Chase);
                break;

            case State.Patrol:
                Patrol();
                if (PlayerDetected()) ChangeState(State.Chase);
                break;

            case State.Chase:
                ChasePlayer(distanceToPlayer);
                break;

            case State.Attack:
                HandleAttack(distanceToPlayer);
                break;

            case State.Return:
                ReturnToOrigin();
                break;
        }

        UpdateMovementDirection();
    }

    protected void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (newState == State.Idle)
            idleTimer = idleTime;
    }

    // ─────────────────────────────
    // STATES BASE
    // ─────────────────────────────

    protected virtual void Idle()
    {
        controller.Stop();
        agent.ResetPath();

        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0f)
        {
            patrolTimer = 0f;
            ChangeState(State.Patrol);
        }
    }

    protected virtual void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        if (!agent.hasPath || agent.remainingDistance < 0.2f || patrolTimer <= 0f)
        {
            Vector3 randomPoint = originPosition +
                (Vector3)(Random.insideUnitCircle * patrolRadius);

            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            patrolTimer = patrolWaitTime;
        }
    }

    protected virtual void ChasePlayer(float distance)
    {
        if (distance > chaseDistance)
        {
            ReleaseAttackSlot();
            ChangeState(State.Return);
            return;
        }

        if (distance <= attackDistance)
        {
            if (TryAcquireAttackSlot())
            {
                agent.velocity = Vector3.zero;
                ChangeState(State.Attack);
            }
            else
            {
                OrbitPlayer();
            }
            return;
        }

        agent.SetDestination(player.position);
    }

    protected virtual void ReturnToOrigin()
    {
        agent.SetDestination(originPosition);

        if (Vector2.Distance(transform.position, originPosition) < 0.3f)
        {
            ChangeState(State.Idle);
        }
    }

    // ─────────────────────────────
    // ABSTRACT
    // ─────────────────────────────

    protected abstract void HandleAttack(float distance);

    // ─────────────────────────────
    // MOVEMENT
    // ─────────────────────────────

    protected void UpdateMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            Vector3 separation = CalculateSeparation();
            controller.SetMoveDirection(agent.velocity + separation);
        }
        else
        {
            controller.Stop();
        }
    }

    protected Vector3 CalculateSeparation()
    {
        Collider2D[] nearby =
            Physics2D.OverlapCircleAll(transform.position, separationRadius, enemyLayer);

        Vector3 force = Vector3.zero;

        foreach (var col in nearby)
        {
            if (col.transform == transform) continue;

            Vector3 dir = transform.position - col.transform.position;
            float dist = dir.magnitude;

            if (dist > 0.01f)
                force += dir.normalized / dist;
        }

        return force * separationStrength;
    }

    protected bool PlayerDetected()
    {
        if (!player) return false;
        return Vector2.Distance(transform.position, player.position) <= detectionDistance;
    }

    protected bool TryAcquireAttackSlot()
    {
        if (hasAttackSlot) return true;
        if (attackSlots == null) return true;

        hasAttackSlot = attackSlots.RequestSlot(this);
        return hasAttackSlot;
    }

    protected void ReleaseAttackSlot()
    {
        if (!hasAttackSlot || attackSlots == null) return;

        attackSlots.ReleaseSlot(this);
        hasAttackSlot = false;
    }

    protected void OrbitPlayer()
    {
        Vector3 dir = transform.position - player.position;
        Vector3 tangent = Vector3.Cross(dir, Vector3.forward).normalized;

        float orbitDistance = attackDistance + 0.5f;
        Vector3 targetPos =
            player.position +
            dir.normalized * orbitDistance +
            tangent * Mathf.Sin(Time.time * 2f);

        agent.SetDestination(targetPos);
    }
}
