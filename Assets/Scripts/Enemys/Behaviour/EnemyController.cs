using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Vector2 moveDirection;
    protected bool canMove = true;

    public bool CanMove => canMove;

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
