using UnityEngine;

public class PlayerPhysic : MonoBehaviour
{
    public Rigidbody2D playerRB;

    [SerializeField] Collider2D col;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float slideOnWallSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpAirPower;
    [SerializeField] private float wallBouncePower;

    Vector2 externalPush;
    float defaultGravityScale;

    void OnValidate()
    {
        playerRB = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<Collider2D>();
    }

    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        defaultGravityScale = playerRB.gravityScale;

        PlayerController.Instance.OnDead += OnDead;
    }

    void OnDead()
    {
        col.enabled = false;
    }

    public void Reset()
    {
        PlayerController.Instance.playerInput.isDead = false;
        col.enabled = true;
        playerRB.linearVelocity = Vector2.zero;
    }

    public void MoveHorizontal(float inputX)
    {
        playerRB.linearVelocity = new Vector2(
            inputX * moveSpeed,
            playerRB.linearVelocity.y
        );
    }

    public void HandleExternalPush(Vector2 move)
    {
        if (!PlayerController.Instance.playerInput.isExternallyPushed) return;

        Vector2 v = playerRB.linearVelocity;

        if (externalPush.x != 0)
        {
            v.x = externalPush.x + move.x * moveSpeed;
        }

        if (externalPush.y != 0)
        {
            v.y = externalPush.y;
        }

        playerRB.linearVelocity = v;
    }

    public void SetExternalPush(Vector2 direction, float power)
    {
        PlayerController.Instance.playerInput.isExternallyPushed = true;
        externalPush = direction.normalized * power;
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            playerRB.gravityScale = 0;
    }

    public void ClearExternalPush()
    {
        playerRB.gravityScale = defaultGravityScale;
        PlayerController.Instance.playerInput.isExternallyPushed = false;
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

    public void ReceiveDamage()
    {
        PlayerController.Instance.playerInput.isDead = true;
        PlayerController.Instance.OnDead?.Invoke();
    }
}