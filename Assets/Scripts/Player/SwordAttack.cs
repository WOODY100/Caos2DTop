using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAttack : MonoBehaviour
{
    [Header("Hitboxes")]
    public GameObject hitboxUp;
    public GameObject hitboxDown;
    public GameObject hitboxLeft;
    public GameObject hitboxRight;

    private Controls controls;
    private PlayerController player;
    private Animator animator;

    private bool isAttacking;
    private GameObject currentHitbox;

    void Awake()
    {
        player = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        controls = new Controls();

        controls.Player.Attack.performed += _ => TryAttack();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void TryAttack()
    {
        if (isAttacking) return;

        isAttacking = true;
        player.SetAttacking(true);

        SelectHitbox();
        animator.SetTrigger("Attack");
    }

    void SelectHitbox()
    {
        DisableAllHitboxes();

        switch (player.currentDirection)
        {
            case Direction.Up:
                currentHitbox = hitboxUp;
                break;
            case Direction.Down:
                currentHitbox = hitboxDown;
                break;
            case Direction.Left:
                currentHitbox = hitboxLeft;
                break;
            case Direction.Right:
                currentHitbox = hitboxRight;
                break;
        }
    }

    // 🎞️ Animation Events
    public void EnableHitbox()
    {
        currentHitbox?.SetActive(true);
    }

    public void DisableHitbox()
    {
        currentHitbox?.SetActive(false);
        DisableAllHitboxes();

        isAttacking = false;
        player.SetAttacking(false);
    }

    void DisableAllHitboxes()
    {
        hitboxUp.SetActive(false);
        hitboxDown.SetActive(false);
        hitboxLeft.SetActive(false);
        hitboxRight.SetActive(false);
    }
}
