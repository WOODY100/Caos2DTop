using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyController))]
[RequireComponent(typeof(EnemyAttack))]
public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        Chase,
        Return,
        Attack
    }

    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Detection")]
    public float detectionDistance = 4f;
    public float chaseDistance = 7f;
    public float attackDistance = 1.1f;

    [Header("Idle")]
    public float idleTime = 1f;
    private float idleTimer;

    [Header("Patrol")]
    public float patrolRadius = 3f;
    public float patrolWaitTime = 1.5f;
    private float patrolTimer;

    private State currentState;

    private NavMeshAgent agent;
    private EnemyController controller;
    private EnemyAttack attack;

    private Vector3 originPosition;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();
        attack = GetComponent<EnemyAttack>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        originPosition = transform.position;

        idleTimer = idleTime;
        currentState = State.Idle;
    }

    void Update()
    {
        float distanceToPlayer = player
            ? Vector2.Distance(transform.position, player.position)
            : Mathf.Infinity;

        switch (currentState)
        {
            case State.Idle:
                Idle();
                if (PlayerDetected())
                    ChangeState(State.Chase);
                break;

            case State.Patrol:
                Patrol();
                if (PlayerDetected())
                    ChangeState(State.Chase);
                break;

            case State.Chase:
                ChasePlayer(distanceToPlayer);
                break;

            case State.Attack:
                TryAttack(distanceToPlayer);
                break;

            case State.Return:
                ReturnToOrigin();
                break;
        }

        UpdateMovementDirection();
    }

    void ChangeState(State newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        if (newState == State.Idle)
            idleTimer = idleTime;
    }

    // ─────────────────────────────
    // STATES
    // ─────────────────────────────

    void Idle()
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

    void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        if (!agent.hasPath || agent.remainingDistance < 0.2f || patrolTimer <= 0f)
        {
            Vector3 randomPoint = originPosition +
                                  (Vector3)(Random.insideUnitCircle * patrolRadius);

            if (NavMesh.SamplePosition(
                randomPoint,
                out NavMeshHit hit,
                patrolRadius,
                NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            patrolTimer = patrolWaitTime;
        }
    }

    void ChasePlayer(float distance)
    {
        if (distance > chaseDistance)
        {
            ChangeState(State.Return);
            return;
        }

        if (distance <= attackDistance)
        {
            agent.ResetPath();
            ChangeState(State.Attack);
            return;
        }

        agent.SetDestination(player.position);
    }

    void TryAttack(float distance)
    {
        if (distance > attackDistance)
        {
            ChangeState(State.Chase);
            return;
        }

        attack.TryAttack();
    }

    void ReturnToOrigin()
    {
        agent.SetDestination(originPosition);

        if (Vector2.Distance(transform.position, originPosition) < 0.3f)
        {
            ChangeState(State.Idle);
        }
    }

    // ─────────────────────────────
    // HELPERS
    // ─────────────────────────────

    bool PlayerDetected()
    {
        if (player == null) return false;

        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= detectionDistance;
    }

    void UpdateMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            controller.SetMoveDirection(agent.velocity);
        }
        else
        {
            controller.Stop();
        }
    }

    // ─────────────────────────────
    // DEBUG
    // ─────────────────────────────

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(originPosition, patrolRadius);
    }
}
