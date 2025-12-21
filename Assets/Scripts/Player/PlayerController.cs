using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 lastMoveDirection = Vector2.down;

    [Header("Direction")]
    public Direction currentDirection = Direction.Down;

    [Header("Attack")]
    public bool IsAttacking { get; private set; }
    public void SetAttacking(bool value)
    {
        IsAttacking = value;
    }

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Controls controls;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new Controls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        UpdateDirection();
    }

    void FixedUpdate()
    {
        if (IsAttacking) return;
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void UpdateDirection()
    {
        if (moveInput == Vector2.zero) return;

        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            if (moveInput.x > 0)
            {
                currentDirection = Direction.Right;
                lastMoveDirection = Vector2.right;
            }
            else
            {
                currentDirection = Direction.Left;
                lastMoveDirection = Vector2.left;
            }
        }
        else
        {
            if (moveInput.y > 0)
            {
                currentDirection = Direction.Up;
                lastMoveDirection = Vector2.up;
            }
            else
            {
                currentDirection = Direction.Down;
                lastMoveDirection = Vector2.down;
            }
        }
    }

    public Vector2 GetAnimationDirection()
    {
        return lastMoveDirection;
    }

    // 👉 Útil para ataque, interacción, etc.
    public Vector2 GetDirectionVector()
    {
        return currentDirection switch
        {
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            _ => Vector2.down
        };
    }

    // 👉 Para animaciones
    public bool IsMoving()
    {
        return moveInput != Vector2.zero;
    }
}
