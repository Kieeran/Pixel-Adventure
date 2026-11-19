using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerController playerController;
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
        playerController.OnDoubleJump += () =>
        {
            animator.SetTrigger(isDoubleJumpHash);
        };
    }

    void Update()
    {
        FlipSprite();

        animator.SetFloat(
            xVelocityHash,
            playerController.playerPhysic.CanMoveHorizontal() ? Mathf.Abs(playerController.playerInput.move.x) : 0
        );
        animator.SetFloat(yVelocityHash, playerController.playerPhysic.playerRB.linearVelocityY);
        animator.SetBool(isGroundedHash, playerController.playerInput.isGrounded);
        animator.SetBool(isOnWallHash, playerController.playerInput.isOnWall);
        animator.SetBool(isDead, playerController.playerInput.isDead);
    }

    void FlipSprite()
    {
        Vector2 move = playerController.playerInput.move;
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
