using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    bool isFacingRight = true;

    private static readonly int xVelocityHash = Animator.StringToHash("xVelocity");
    private static readonly int yVelocityHash = Animator.StringToHash("yVelocity");
    private static readonly int isGroundedHash = Animator.StringToHash("isGrounded");
    private static readonly int isDoubleJumpHash = Animator.StringToHash("isDoubleJump");

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

        animator.SetFloat(xVelocityHash, Mathf.Abs(PlayerController.Instance.playerMovement.playerRB.linearVelocityX));
        animator.SetFloat(yVelocityHash, PlayerController.Instance.playerMovement.playerRB.linearVelocityY);
        animator.SetBool(isGroundedHash, PlayerController.Instance.playerInput.isGrounded);
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
}
