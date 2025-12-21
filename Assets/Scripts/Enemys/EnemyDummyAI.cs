using UnityEngine;

public class EnemyDummyAI : EnemyController
{
    protected override void Awake()
    {
        base.Awake();
        InvokeRepeating(nameof(ChangeDirection), 1f, 2f);
    }

    void ChangeDirection()
    {
        Vector2[] dirs = {
            Vector2.up, Vector2.down,
            Vector2.left, Vector2.right,
            Vector2.zero
        };

        SetMoveDirection(dirs[Random.Range(0, dirs.Length)]);
    }
}
