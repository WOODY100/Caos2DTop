using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down;
    public bool canMove = true;

    [Header("Direction")]
    public Direction currentDirection = Direction.Down;

    [Header("Attack")]
    public bool IsAttacking { get; private set; }

    private Rigidbody2D rb;
    private Controls controls;

    public void SetAttacking(bool value)
    {
        IsAttacking = value;

        if (value)
        {
            // 🔑 FRENAR DESLIZAMIENTO
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = InputManager.Instance.Controls;
    }

    private void OnEnable()
    {
        Time.timeScale = 1f; // 🔑 failsafe

        // INPUT
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMoveCanceled;

        // 🔑 RESET FÍSICA (CLAVE)
        rb.simulated = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        canMove = true;
        IsAttacking = false;

        // LevelUp
        var exp = GetComponent<PlayerExperience>();
        var stats = GetComponent<PlayerStats>();

        if (LevelUpManager.Instance != null && exp != null && stats != null)
            LevelUpManager.Instance.RegisterPlayer(exp, stats);
    }

    private void OnDisable()
    {
        // 🔑 LIMPIEZA INPUT
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMoveCanceled;

        var exp = GetComponent<PlayerExperience>();
        if (LevelUpManager.Instance != null && exp != null)
            LevelUpManager.Instance.UnregisterPlayer(exp);
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();

        if (!GameStateManager.Instance.IsGameplayAllowed())
            return;

        moveInput = value;
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        CancelMovement();
    }

    private void CancelMovement()
    {
        moveInput = Vector2.zero;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public void ForceCancelMovement()
    {
        CancelMovement();
    }

    void Update()
    {

        if (!GameStateManager.Instance.IsGameplayAllowed())
            return;

        UpdateDirection();
    }

    void FixedUpdate()
    {
        if (!canMove) return;
        if (IsAttacking) return;
        if (!GameStateManager.Instance.IsGameplayAllowed())
            return;

        rb.linearVelocity = moveInput * moveSpeed;
    }

    void UpdateDirection()
    {
        if (moveInput == Vector2.zero || !canMove || IsAttacking)
            return;

        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            currentDirection = moveInput.x > 0 ? Direction.Right : Direction.Left;
        else
            currentDirection = moveInput.y > 0 ? Direction.Up : Direction.Down;

        lastMoveDirection = GetDirectionVector();
    }

    public Vector2 GetAnimationDirection()
    {
        return lastMoveDirection;
    }

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

    public bool IsMoving()
    {
        return moveInput != Vector2.zero;
    }

    public void SetInputEnabled(bool value)
    {
        canMove = value;
    }
}
