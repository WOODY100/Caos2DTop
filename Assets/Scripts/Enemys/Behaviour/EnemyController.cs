using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;

    protected Rigidbody2D rb;
    protected Vector2 moveDirection;
    protected bool canMove = true;

    public bool CanMove => canMove;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (!canMove) return;

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetMoveDirection(Vector2 dir)
    {
        moveDirection = dir == Vector2.zero ? Vector2.zero : dir.normalized;
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public bool IsMoving()
    {
        return moveDirection != Vector2.zero;
    }

    public void Stop()
    {
        moveDirection = Vector2.zero;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
