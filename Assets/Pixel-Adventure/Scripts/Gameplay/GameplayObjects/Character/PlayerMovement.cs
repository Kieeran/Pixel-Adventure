using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D playerRB;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float slideOnWallSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpAirPower;
    [SerializeField] private float wallBouncePower;
    [SerializeField] private float KnockUpByBoxPower;
    [SerializeField] private float KnockDownByBoxPower;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
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

    public void KnockBackByBox(bool knockUp)
    {
        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
        if (knockUp)
        {
            playerRB.AddForce(Vector2.up * KnockUpByBoxPower, ForceMode2D.Impulse);
        }
        else
        {
            playerRB.AddForce(Vector2.down * KnockDownByBoxPower, ForceMode2D.Impulse);
        }
    }

    public void PushUpByTrampoline(float force)
    {
        playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0);
        playerRB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }
}