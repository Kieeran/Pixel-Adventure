using System.Collections.Generic;
using UnityEngine;

public class RockHead : PlacedObject
{
    public enum MovementDirection { Up, Down, Left, Right }
    public enum TerrainCollisionSide
    {
        Left = 0,
        Right = 1,
        Top = 2,
        Bottom = 3
    }

    private static readonly int isHitHash = Animator.StringToHash("isHit");
    private static readonly int hitSideHash = Animator.StringToHash("hitSide");
    [SerializeField] RockHeadCollision rockHeadCollision;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float initialSpeed = 1f;
    [Min(0f)] public float delayTime = 1f;
    [SerializeField] float knockBackForce = 1f;
    public List<MovementDirection> directions = new();

    int currentDirectionIndex;
    bool isWaiting;
    float waitTimer;
    float currentSpeed;
    private bool isCarryingPlayer;
    private bool isClearExternalForce;

    void OnValidate()
    {
        rockHeadCollision = GetComponent<RockHeadCollision>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    public override void OnSpawn()
    {
        rockHeadCollision.OnCharacterRidingStateChanged += HandleCharacterRidingStateChanged;
        rockHeadCollision.OnCharacterCrushed += HandleCharacterCrushed;

        if (customData is RockHeadData data)
        {
            SetDirections(data.directions);
            delayTime = data.delayTime;
        }
        else
        {
            SetDirections(null);
        }

        currentDirectionIndex = 0;
        isWaiting = false;
        waitTimer = 0f;
        currentSpeed = initialSpeed;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        isCarryingPlayer = false;
        isClearExternalForce = false;
    }

    public override void OnDespawn()
    {
        rockHeadCollision.OnCharacterRidingStateChanged -= HandleCharacterRidingStateChanged;
        rockHeadCollision.OnCharacterCrushed -= HandleCharacterCrushed;

        rb.linearVelocity = Vector2.zero;
        currentDirectionIndex = 0;
        isWaiting = false;
        waitTimer = 0f;
        currentSpeed = 0f;
        isClearExternalForce = false;
        if (isCarryingPlayer)
        {
            PlayerController.Instance.playerPhysic.ClearExternalForce();
            isCarryingPlayer = false;
        }
    }

    void FixedUpdate()
    {
        if (directions.Count == 0 || maxSpeed <= 0f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isCarryingPlayer)
        {
            PlayerController.Instance.playerPhysic.SetExternalForce(GetCurrentMovementDirection(), currentSpeed);
        }
        else
        {
            if (!isClearExternalForce)
            {
                PlayerController.Instance.playerPhysic.ClearExternalForce();
                isClearExternalForce = true;
            }
        }

        if (isWaiting)
        {
            rb.linearVelocity = Vector2.zero;
            waitTimer += Time.fixedDeltaTime;
            if (waitTimer >= Mathf.Max(0f, delayTime))
            {
                waitTimer = 0f;
                currentDirectionIndex = (currentDirectionIndex + 1) % directions.Count;
                currentSpeed = initialSpeed;
                isWaiting = false;
            }
            return;
        }

        rb.linearVelocity = GetCurrentMovementDirection() * currentSpeed;
        currentSpeed = Mathf.Min(
            currentSpeed + Mathf.Max(0f, acceleration) * Time.fixedDeltaTime,
            Mathf.Max(0f, maxSpeed)
        );
    }

    void HandleCharacterRidingStateChanged(bool isRiding)
    {
        isCarryingPlayer = isRiding;
        isClearExternalForce = isRiding;
    }

    void HandleCharacterCrushed(Vector2 hitDirection)
    {
        PlayerController.Instance.playerPhysic.ReceiveDamage(Vector2.zero, knockBackForce, hitDirection);
    }

    public void OnTerrainCollision(TerrainCollisionSide hitSide, Vector2 overlappedRetrieveOffset)
    {
        if (isWaiting || directions.Count == 0) return;

        isWaiting = true;
        waitTimer = 0f;
        currentSpeed = 0f;
        rb.linearVelocity = Vector2.zero;

        rb.position += overlappedRetrieveOffset;

        animator.SetTrigger(isHitHash);
        animator.SetInteger(hitSideHash, (int)hitSide);
    }

    public bool IsBlockedByTerrain(Vector2 contactNormal)
    {
        if (isWaiting || directions.Count == 0) return false;

        Vector2 movementDirection = GetCurrentMovementDirection();
        // Bỏ qua va chạm theo trục vuông góc với hướng di chuyển để không dừng nhầm.
        bool isHorizontalMovement = movementDirection.x != 0f;
        bool isHorizontalContact = Mathf.Abs(contactNormal.x) > Mathf.Abs(contactNormal.y);

        // Tích vô hướng âm nghĩa là pháp tuyến hướng ngược chiều di chuyển,
        // tức bề mặt terrain đang chặn RockHead.
        return isHorizontalMovement == isHorizontalContact
            && Vector2.Dot(movementDirection, contactNormal) < 0f;
    }

    public void SetDirections(List<MovementDirection> newDirections)
    {
        directions = newDirections == null
            ? new List<MovementDirection>()
            : new List<MovementDirection>(newDirections);
    }

    static Vector2 GetDirectionVector(MovementDirection direction)
    {
        return direction switch
        {
            MovementDirection.Up => Vector2.up,
            MovementDirection.Down => Vector2.down,
            MovementDirection.Left => Vector2.left,
            MovementDirection.Right => Vector2.right,
            _ => Vector2.zero
        };
    }

    public Vector2 GetCurrentMovementDirection()
    {
        return GetDirectionVector(directions[currentDirectionIndex]);
    }
}
