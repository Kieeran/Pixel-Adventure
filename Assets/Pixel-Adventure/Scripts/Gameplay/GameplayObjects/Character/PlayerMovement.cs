using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRB;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float slideOnWallSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpAirPower;
    [SerializeField] private float wallBouncePower;
    [SerializeField] private bool isFacingRight = true;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        // PlayerController.Instance.OnJump += OnJump;
    }

    private void Update()
    {
        UpdateSpriteDirection();
    }

    private void FixedUpdate()
    {
        // Move();
    }

    void Move()
    {
        Vector2 currentVelocity = playerRB.linearVelocity;

        currentVelocity.x = PlayerController.Instance.playerInput.move.x * moveSpeed;

        if (PlayerController.Instance.playerInput.isOnWall == true)
        {
            currentVelocity.y = -slideOnWallSpeed;
        }

        playerRB.linearVelocity = currentVelocity;
    }

    public void MoveHorizontal(float inputX)
    {
        playerRB.linearVelocity = new Vector2(
            inputX * moveSpeed,
            playerRB.linearVelocity.y
        );
    }

    public void Jump()
    {
        playerRB.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    public void JumpInAir()
    {
        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
        playerRB.AddForce(Vector2.up * jumpAirPower, ForceMode2D.Impulse);
    }

    public void JumpFromWall()
    {
        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
        playerRB.AddForce(
            (PlayerController.Instance.playerInput.isContactLeftWall ? Vector2.right : Vector2.left) * wallBouncePower + Vector2.up * jumpPower,
            ForceMode2D.Impulse
        );
    }

    public void SlideOnWall()
    {
        playerRB.linearVelocity = new Vector2(
            playerRB.linearVelocity.x,
            -slideOnWallSpeed
        );
    }

    void OnJump()
    {
        if (PlayerController.Instance.playerInput.isGrounded == true)
        {
            playerRB.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }

        if (PlayerController.Instance.playerInput.isJumpInAir == false && PlayerController.Instance.playerInput.isGrounded == false)
        {
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);

            playerRB.AddForce(Vector2.up * jumpAirPower, ForceMode2D.Impulse);
            PlayerController.Instance.playerInput.isJumpInAir = true;
            PlayerController.Instance.OnDoubleJump?.Invoke();
        }
    }

    private void UpdateSpriteDirection()
    {
        Vector2 _move = PlayerController.Instance.playerInput.move;
        if (isFacingRight && _move.x < 0f || !isFacingRight && _move.x > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }
}
