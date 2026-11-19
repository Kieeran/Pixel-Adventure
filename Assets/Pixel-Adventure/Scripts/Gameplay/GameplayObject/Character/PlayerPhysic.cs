using UnityEngine;

public class PlayerPhysic : MonoBehaviour
{
    public PlayerController playerController;
    public Rigidbody2D playerRB;

    [SerializeField] Collider2D col;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float slideOnWallSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpAirPower;
    [SerializeField] private float wallBouncePower;

    [Header("Knock Back")]
    [SerializeField] private float verticalHitThreshold = 10f;
    [SerializeField] private float verticalTiltAngle = 17.5f;
    [SerializeField] private float downwardKnockBackMultiplier = 0.0f;
    [SerializeField] private float deadSpinSpeed = 180f;
    [SerializeField] private float deadSpinAngle = 17.5f;

    Vector2 externalForce;
    Vector2 resistance;
    float slideVelocityX = 0f;
    readonly float offset = 1;
    float defaultGravityScale;
    RigidbodyConstraints2D defaultConstraints;

    void OnValidate()
    {
        playerRB = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<Collider2D>();
    }

    void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        defaultGravityScale = playerRB.gravityScale;
        defaultConstraints = playerRB.constraints;

        playerController.OnDead += OnDead;
    }

    void OnDead()
    {
        col.enabled = false;
        Invoke(nameof(DisablePlayer), 1f);
    }

    void DisablePlayer()
    {
        playerController.playerAnimation.ToggleRenderer(false);
        playerRB.gravityScale = 0;
        playerRB.linearVelocity = Vector2.zero;
        playerRB.angularVelocity = 0;
        playerRB.position = Vector2.zero;
        playerRB.rotation = 0;

        EventChannel.Instance.OnPlayerDisabled?.Invoke();
    }

    public void Reset()
    {
        playerController.playerAnimation.ToggleRenderer(true);
        playerController.playerInput.isDead = false;
        playerRB.gravityScale = defaultGravityScale;
        playerRB.constraints = defaultConstraints;
        col.enabled = true;
    }

    public void MoveHorizontal(float inputX)
    {
        if (playerController.playerInput.isDead) return;

        playerRB.linearVelocity = new Vector2(
            inputX * moveSpeed,
            playerRB.linearVelocity.y
        );
    }

    public void HandleExternalForce(Vector2 move)
    {
        if (!playerController.playerInput.isExternallyForced) return;
        if (playerController.playerInput.isDead) return;

        Vector2 v = playerRB.linearVelocity;

        if (externalForce.x != 0)
        {
            v.x = externalForce.x + move.x * moveSpeed;
        }

        if (externalForce.y != 0)
        {
            v.y = externalForce.y;
        }

        playerRB.linearVelocity = v;
    }

    public void HandleResistance()
    {
        if (!playerController.playerInput.isResisted) return;
        if (playerController.playerInput.isDead) return;

        Vector2 v = playerRB.linearVelocity;

        v.x *= 1 - resistance.x;
        v.y *= 1 - resistance.y;

        playerRB.linearVelocity = v;
    }

    public void HandleSlide()
    {
        if (playerController.playerInput.isDead) return;
        if (!playerController.playerInput.isGrounded)
        {
            slideVelocityX = 0;
            return;
        }

        if (playerRB.linearVelocityX > 0)
        {
            slideVelocityX += offset;
            if (slideVelocityX > moveSpeed)
            {
                slideVelocityX = moveSpeed;
            }
        }

        else if (playerRB.linearVelocityX < 0)
        {
            slideVelocityX -= offset;
            if (slideVelocityX < -moveSpeed)
            {
                slideVelocityX = -moveSpeed;
            }
        }

        if (!playerController.playerInput.isSliding) return;

        playerRB.linearVelocity = new Vector2(slideVelocityX, playerRB.linearVelocityY);
    }

    public void Sliding()
    {
        playerController.playerInput.isSliding = true;
    }

    public void StopSliding()
    {
        playerController.playerInput.isSliding = false;
        slideVelocityX = 0;
    }

    public void SetExternalForce(Vector2 direction, float power)
    {
        playerController.playerInput.isExternallyForced = true;
        externalForce = direction.normalized * power;
        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
            playerRB.gravityScale = 0;
    }

    public void ClearExternalForce()
    {
        playerRB.gravityScale = defaultGravityScale;
        playerController.playerInput.isExternallyForced = false;
    }

    public void ClearResistance()
    {
        if (resistance.y == 1)
        {
            playerRB.gravityScale = defaultGravityScale;
        }

        resistance = Vector2.zero;
        playerController.playerInput.isResisted = false;
    }

    public void ResistMovement(Vector2 resistance)
    {
        this.resistance = resistance;

        if (resistance.y == 1)
        {
            playerRB.gravityScale = 0;
        }

        playerController.playerInput.isResisted = true;
    }

    public bool CanMoveHorizontal()
    {
        return resistance.x != 1;
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
            (playerController.playerInput.isContactLeftWall ? Vector2.right : Vector2.left) * wallBouncePower + Vector2.up * jumpPower,
            ForceMode2D.Impulse
        );

        // Chổ này phải set thủ công ở đây vì nếu không qua fram FixedUpdate sau sẽ đi qua hàm HandleResistance() reset lại lực vertical
        playerController.playerInput.isResisted = false;
    }

    public void SlideOnWall()
    {
        if (playerController.playerInput.isDead) return;

        // Resistance theo chiều dọc chỉ áp dụng khi player trượt trên tường
        if (playerController.playerInput.isResisted && resistance.y == 1) return;

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

    public void ReceiveDamage(Vector2 hitPos, float knockBackForce, Vector2 direction = default)
    {
        // Lấy tâm trước khi OnDead tắt collider
        Vector2 center = col.bounds.center;

        playerController.playerInput.isDead = true;
        playerController.OnDead?.Invoke();

        if (direction != default)
        {
            KnockBack(direction, knockBackForce);
        }
        else
        {
            KnockBack(center - hitPos, knockBackForce);
        }
    }

    void KnockBack(Vector2 direction, float force)
    {
        ClearExternalForce();

        Vector2 dir = direction.normalized;

        // Va chạm gần như thẳng đứng thì nghiêng sang trái/phải ngẫu nhiên
        // Hướng xuống trùng gravity nên giảm lực, hướng bằng 0 cũng tính vào trường hợp này
        if (dir == Vector2.zero || Vector2.Angle(dir, Vector2.down) < verticalHitThreshold)
        {
            dir = TiltRandomly(Vector2.down);
            force *= downwardKnockBackMultiplier;
        }
        else if (Vector2.Angle(dir, Vector2.up) < verticalHitThreshold)
        {
            dir = TiltRandomly(Vector2.up);
        }

        playerRB.linearVelocity = Vector2.zero;
        playerRB.AddForce(dir * force, ForceMode2D.Impulse);

        // Phải mở freeze rotation trước, nếu không angularVelocity không có tác dụng
        playerRB.freezeRotation = false;
        playerRB.angularVelocity = RandomSign() * deadSpinSpeed;
    }

    public void LimitDeadSpin()
    {
        if (Mathf.Abs(playerRB.rotation) < deadSpinAngle) return;

        playerRB.angularVelocity = 0;
        playerRB.rotation = Mathf.Sign(playerRB.rotation) * deadSpinAngle;
    }

    Vector2 TiltRandomly(Vector2 v)
    {
        return Quaternion.Euler(0, 0, RandomSign() * verticalTiltAngle) * v;
    }

    static float RandomSign()
    {
        return Random.value < 0.5f ? -1f : 1f;
    }
}