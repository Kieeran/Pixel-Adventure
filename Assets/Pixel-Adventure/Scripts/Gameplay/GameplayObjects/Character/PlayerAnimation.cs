using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;

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
        animator.SetFloat(xVelocityHash, Mathf.Abs(PlayerController.Instance.playerMovement.playerRB.linearVelocityX));
        animator.SetFloat(yVelocityHash, PlayerController.Instance.playerMovement.playerRB.linearVelocityY);
        animator.SetBool(isGroundedHash, PlayerController.Instance.playerInput.isGrounded);
    }
}
