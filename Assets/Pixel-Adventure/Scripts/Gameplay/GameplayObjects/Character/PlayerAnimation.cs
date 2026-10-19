using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    bool isFacingRight = true;

    private static readonly int xVelocityHash = Animator.StringToHash("xVelocity");
    private static readonly int yVelocityHash = Animator.StringToHash("yVelocity");
    private static readonly int isGroundedHash = Animator.StringToHash("isGrounded");
    private static readonly int isDoubleJumpHash = Animator.StringToHash("isDoubleJump");
    private static readonly int isOnWallHash = Animator.StringToHash("isOnWall");
    private static readonly int isDead = Animator.StringToHash("isDead");

    void OnValidate()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        PlayerController.Instance.OnDoubleJump += () =>
        {
            animator.SetTrigger(isDoubleJumpHash);
        };
    }

    void Update()
    {
        FlipSprite();

        animator.SetFloat(
            xVelocityHash,
            PlayerController.Instance.playerPhysic.CanMoveHorizontal() ? Mathf.Abs(PlayerController.Instance.playerInput.move.x) : 0
        );
        animator.SetFloat(yVelocityHash, PlayerController.Instance.playerPhysic.playerRB.linearVelocityY);
        animator.SetBool(isGroundedHash, PlayerController.Instance.playerInput.isGrounded);
        animator.SetBool(isOnWallHash, PlayerController.Instance.playerInput.isOnWall);
        animator.SetBool(isDead, PlayerController.Instance.playerInput.isDead);
    }

    void FlipSprite()
    {
        Vector2 move = PlayerController.Instance.playerInput.move;
        if (isFacingRight && move.x < 0f || !isFacingRight && move.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    public void ToggleRenderer(bool b)
    {
        spriteRenderer.enabled = b;
    }
}
