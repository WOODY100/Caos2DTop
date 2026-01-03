using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State
    {
        Patrol,
        Chase,
        Return,
        Attack
    }

    [Header("Detection")]
    public float detectionDistance = 4f;
    public float chaseDistance = 7f;
    public float attackDistance = 1.1f;

    [Header("Patrol")]
    public float patrolRadius = 3f;
    public float patrolWaitTime = 1.5f;

    private State currentState;
    private Transform player;
    private NavMeshAgent agent;
    private EnemyController controller;
    private EnemyAttack attack;

    private Vector3 originPosition;
    private float patrolTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyController>();
        attack = GetComponent<EnemyAttack>();

        originPosition = transform.position;
        currentState = State.Patrol;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (distanceToPlayer <= detectionDistance)
                    currentState = State.Chase;
                break;

            case State.Chase:
                ChasePlayer(distanceToPlayer);
                break;

            case State.Return:
                ReturnToOrigin();
                break;

            case State.Attack:
                TryAttack(distanceToPlayer);
                break;
        }

        UpdateMovementDirection();
    }

    void Patrol()
    {
        patrolTimer -= Time.deltaTime;

        if (!agent.hasPath || patrolTimer <= 0f)
        {
            Vector3 randomPoint = originPosition + (Vector3)(Random.insideUnitCircle * patrolRadius);
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
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
            currentState = State.Return;
            return;
        }

        if (distance <= attackDistance)
        {
            agent.ResetPath();
            currentState = State.Attack;
            return;
        }

        agent.SetDestination(player.position);
    }

    void TryAttack(float distance)
    {
        if (distance > attackDistance)
        {
            currentState = State.Chase;
            return;
        }

        attack.TryAttack();
    }

    void ReturnToOrigin()
    {
        agent.SetDestination(originPosition);

        if (Vector2.Distance(transform.position, originPosition) < 0.3f)
        {
            currentState = State.Patrol;
        }
    }

    void UpdateMovementDirection()
    {
        Vector2 velocity = agent.velocity;
        controller.SetMoveDirection(velocity);
    }
}
