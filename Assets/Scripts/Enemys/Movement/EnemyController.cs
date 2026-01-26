using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    
    [Header("Facing")]
    [SerializeField] private FacingDirection facing = FacingDirection.Down;
    [SerializeField] private float facingThreshold = 0.2f;
    public FacingDirection Facing => facing;
    private bool facingLocked;

    public enum FacingDirection
    {
        Down,
        Up,
        Left,
        Right
    }

    protected Vector2 moveDirection;
    protected bool canMove = true;
    protected Vector2 lastMoveDirection = Vector2.down;
    public Vector2 LastMoveDirection => lastMoveDirection;

    private Rigidbody2D rb;
    private NavMeshAgent agent;

    [SerializeField] private float moveSpeed = 1f;

    public bool CanMove => canMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            SyncAgent();
            return;
        }

        rb.linearVelocity = moveDirection * moveSpeed;
        SyncAgent();
    }

    private void SyncAgent()
    {
        if (agent != null && agent.enabled)
        {
            agent.nextPosition = rb.position;
        }
    }

    public void SetMoveDirection(Vector2 dir)
    {
        // 🔥 ACTUALIZAR FACING CON VECTOR CRUDO
        if (dir != Vector2.zero && !facingLocked)
        {
            UpdateFacing(dir);
        }

        Vector2 snapped = SnapToCardinal(dir);

        if (snapped != Vector2.zero)
            lastMoveDirection = snapped;

        moveDirection = snapped;
    }

    private void UpdateFacing(Vector2 dir)
    {
        float absX = Mathf.Abs(dir.x);
        float absY = Mathf.Abs(dir.y);

        // 🔒 BLOQUEO DE FACING CON UMBRAL
        if (absX > absY + facingThreshold)
        {
            facing = dir.x > 0
                ? FacingDirection.Right
                : FacingDirection.Left;
        }
        else if (absY > absX + facingThreshold)
        {
            facing = dir.y > 0
                ? FacingDirection.Up
                : FacingDirection.Down;
        }
        // else → NO cambiar facing
    }

    public void LockFacing()
    {
        facingLocked = true;
    }

    public void UnlockFacing()
    {
        facingLocked = false;
    }

    private Vector2 SnapToCardinal(Vector2 input)
    {
        if (input == Vector2.zero)
            return Vector2.zero;

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            return new Vector2(Mathf.Sign(input.x), 0f);
        else
            return new Vector2(0f, Mathf.Sign(input.y));
    }

    public Vector2 GetMoveDirection() => moveDirection;

    public bool IsMoving() => moveDirection != Vector2.zero;

    public void Stop()
    {
        moveDirection = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetFacingFromVector(Vector2 dir)
    {
        if (dir == Vector2.zero)
            return;

        UpdateFacing(dir);

        Vector2 snapped = SnapToCardinal(dir);
        lastMoveDirection = snapped;
    }
}
