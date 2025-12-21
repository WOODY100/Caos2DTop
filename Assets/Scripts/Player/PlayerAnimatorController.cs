using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;
    private PlayerController player;

    void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }

    void Update()
    {
        Vector2 animDir = player.GetAnimationDirection();

        animator.SetFloat("MoveX", animDir.x);
        animator.SetFloat("MoveY", animDir.y);
        animator.SetBool("IsMoving", player.IsMoving());
    }
}
