using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRB;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float slideOnWallSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpAirPower;
    [SerializeField] private float wallBouncePower;

    Vector2 fanPushDirection;
    float currentVelocityX;

    float defaultGravityScale;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        defaultGravityScale = playerRB.gravityScale;
    }

    public void MoveHorizontal(float inputX)
    {
        // playerRB.linearVelocity = new Vector2(
        //     inputX * moveSpeed,
        //     playerRB.linearVelocity.y
        // );
        Vector2 currentVelocity = playerRB.linearVelocity;
        if (PlayerController.Instance.playerInput.isPushedByFan && (fanPushDirection == Vector2.left || fanPushDirection == Vector2.right))
        {
            if (fanPushDirection == Vector2.left)
            {
                currentVelocity.x = inputX < 0 ? currentVelocityX - inputX * moveSpeed : currentVelocityX;
            }
            else if (fanPushDirection == Vector2.right)
            {
                currentVelocity.x = inputX > 0 ? currentVelocityX + inputX * moveSpeed : currentVelocityX;
            }
        }
        else
        {
            currentVelocity.x = inputX * moveSpeed;
        }
        playerRB.linearVelocity = currentVelocity;
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

    public void ReboundVertically(Vector2 direction, float force)
    {
        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
        playerRB.AddForce(direction * force, ForceMode2D.Impulse);
    }

    public void StartFanPush(Vector2 pushDirection)
    {
        Vector2 currentVelocity = playerRB.linearVelocity;
        if (pushDirection == Vector2.up || pushDirection == Vector2.down)
        {
            currentVelocity.y = 0;
            playerRB.gravityScale = 0;
        }
        else if (pushDirection == Vector2.left || pushDirection == Vector2.right)
        {
            currentVelocity.x = 0;
        }

        playerRB.linearVelocity = currentVelocity;
        PlayerController.Instance.playerInput.isPushedByFan = true;
        fanPushDirection = pushDirection;
    }

    public void ApplyFanPush(Vector2 pushDirection, float pushPower)
    {
        Vector2 currentVelocity = playerRB.linearVelocity;
        if (pushDirection == Vector2.up || pushDirection == Vector2.down)
        {
            currentVelocity.y = pushPower;
        }
        else if (pushDirection == Vector2.left || pushDirection == Vector2.right)
        {
            // currentVelocity.x = pushDirection == Vector2.left ? -pushPower : pushPower;
            currentVelocityX = pushDirection == Vector2.left ? -pushPower : pushPower;
        }
        playerRB.linearVelocity = currentVelocity;
    }

    public void StopFanPush()
    {
        // playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
        playerRB.gravityScale = defaultGravityScale;
        PlayerController.Instance.playerInput.isPushedByFan = false;
        fanPushDirection = Vector2.zero;
    }
}