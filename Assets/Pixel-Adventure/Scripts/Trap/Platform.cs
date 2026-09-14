using System.Collections.Generic;
using UnityEngine;

public class Platform : PlacedObject
{
    private static readonly int IsOnHash = Animator.StringToHash("isOn");
    [SerializeField] Animator animator;
    [SerializeField] PlatformCollision platformCollision;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] bool controlledByPlayer;
    public Transform brownSkin;
    public Transform greySkin;
    public List<Vector2> waypoints;
    public float moveSpeed = 1f;
    public float waitTime = 1f;

    private bool isMoving;
    private int currentWaypointIndex;
    private int direction;
    private Vector2 targetPosition;
    private Vector2 moveDirection;
    private float waitTimer;

    private bool enableCollision = true;
    private float delayTime = 0.1f;
    private bool hasPendingCollisionEvent;
    private bool pendingCollisionValue;

    void OnValidate()
    {
        rb = GetComponent<Rigidbody2D>();
        platformCollision = GetComponentInChildren<PlatformCollision>();
    }

    public void SetPlatformSkin(bool isBrownPlatform)
    {
        brownSkin.gameObject.SetActive(isBrownPlatform);
        greySkin.gameObject.SetActive(!isBrownPlatform);

        animator = isBrownPlatform ? brownSkin.GetComponent<Animator>() : greySkin.GetComponent<Animator>();
        controlledByPlayer = isBrownPlatform;
    }

    public override void OnSpawn()
    {
        platformCollision.OnCharacterCollided += OnCharacterCollided;

        PlatformData data = customData as PlatformData;
        SetPlatformSkin(data.isBrownPlatform);
        waypoints = new List<Vector2>(data.waypoints);

        InitWaypointMovement();
    }

    public override void OnDespawn()
    {
        platformCollision.OnCharacterCollided -= OnCharacterCollided;

        brownSkin.gameObject.SetActive(true);
        greySkin.gameObject.SetActive(true);
        waypoints.Clear();
        waitTimer = 0f;

        isMoving = false;
        enableCollision = true;
        hasPendingCollisionEvent = false;
        CancelInvoke();
    }

    protected virtual void FixedUpdate()
    {
        if (waypoints.Count < 2) return;

        if (controlledByPlayer)
            HandleManualPlatform();
        else
            HandleAutomaticPlatform();

        rb.linearVelocity = moveDirection * moveSpeed;
    }

    void HandleManualPlatform()
    {
        if (isMoving)
        {
            float step = moveSpeed * Time.fixedDeltaTime;
            if (Vector2.Distance(rb.position, targetPosition) <= step)
            {
                rb.position = targetPosition;
                currentWaypointIndex = GetNextWaypointIndex();
                PrepareNextWaypoint();

                // Platform đã chở player đến end
                int nextIndex = GetNextWaypointIndex();
                if (nextIndex < 0 || nextIndex >= waypoints.Count)
                {
                    isMoving = false;
                    animator.SetBool(IsOnHash, isMoving);
                    moveDirection = Vector2.zero;
                    enableCollision = false;
                    Invoke(nameof(EnableCollision), delayTime);
                }
            }
        }
    }

    void HandleAutomaticPlatform()
    {
        if (!isMoving)
        {
            waitTimer += Time.fixedDeltaTime;
            if (waitTimer >= waitTime)
            {
                isMoving = true;
                animator.SetBool(IsOnHash, isMoving);
                waitTimer = 0f;
                PrepareNextWaypoint();
            }
            return;
        }

        float step = moveSpeed * Time.fixedDeltaTime;
        if (Vector2.Distance(rb.position, targetPosition) <= step)
        {
            rb.position = targetPosition;
            currentWaypointIndex = GetNextWaypointIndex();
            moveDirection = Vector2.zero;

            isMoving = false;
            animator.SetBool(IsOnHash, isMoving);

            return;
        }
    }

    private void InitWaypointMovement()
    {
        currentWaypointIndex = 0;
        direction = waypoints.Count > 1 ? 1 : 0;

        if (waypoints.Count == 0) return;
        if (waypoints.Count > 1)
        {
            targetPosition = waypoints[GetNextWaypointIndex()];
            if (!controlledByPlayer)
            {
                isMoving = true;
                animator.SetBool(IsOnHash, isMoving);
                UpdateVelocityTowardsTarget();
            }
        }
    }

    private int GetNextWaypointIndex()
    {
        return currentWaypointIndex + direction;
    }

    private void PrepareNextWaypoint()
    {
        int nextIndex = GetNextWaypointIndex();
        if (nextIndex < 0 || nextIndex >= waypoints.Count)
        {
            if (controlledByPlayer) return;

            direction *= -1;
        }

        targetPosition = waypoints[GetNextWaypointIndex()];
        UpdateVelocityTowardsTarget();
    }

    private void UpdateVelocityTowardsTarget()
    {
        if (controlledByPlayer)
        {
            moveDirection = (targetPosition - rb.position).normalized;
        }
        else
        {
            moveDirection = (targetPosition - waypoints[currentWaypointIndex]).normalized;
        }
    }

    public void EnableCollision()
    {
        enableCollision = true;

        // Nếu trong lúc delay có sự kiện nào đang xảy ra -> phát lại sự kiện đó (không bỏ lỡ)
        if (hasPendingCollisionEvent)
        {
            hasPendingCollisionEvent = false;
            HandleCharacterCollided(pendingCollisionValue);
        }
    }

    void OnCharacterCollided(bool isCharacterCollided)
    {
        if (!controlledByPlayer) return;

        // Ghi lại sự kiện mới nhất khi đang delay collision
        if (!enableCollision)
        {
            hasPendingCollisionEvent = true;
            pendingCollisionValue = isCharacterCollided;
            return;
        }

        HandleCharacterCollided(isCharacterCollided);
    }

    void HandleCharacterCollided(bool isCharacterCollided)
    {
        // Với manual platform:
        //
        // Nếu platform đang không di chuyển thì có 2 trường hợp:
        // -> Platform đã chở player đến waypoint cuối cùng - end
        // -> Platform đứng ở vị trí start
        if (!isMoving)
        {
            isMoving = true;
            animator.SetBool(IsOnHash, isMoving);

            // Platform ở vị trí start -> player chỉ vừa nhảy lên trên plaftorm
            if (isCharacterCollided)
            {
                // Chuẩn bị di chuyển từ start tới end
                direction = 1;
                currentWaypointIndex = 0;
                targetPosition = waypoints[GetNextWaypointIndex()];
            }
            // Platform ở vị trí end -> player nhảy ra khỏi platform sau khi đã được chở đến end
            else
            {
                // Chuẩn bị di chuyển từ end về start
                direction = -1;
                currentWaypointIndex = waypoints.Count - 1;
                targetPosition = waypoints[GetNextWaypointIndex()];
            }
        }
        // Nếu platform đang di chuyển
        else
        {
            // Platform đang trên đường quay về start thì player nhảy lên
            if (isCharacterCollided)
            {
                currentWaypointIndex = GetNextWaypointIndex();
                direction = 1;
            }
            // Platform đang chở player tới end thì player rời đi
            else
            {
                currentWaypointIndex = GetNextWaypointIndex();
                direction = -1;
            }
            // Logic trong lệnh if/else bên trên thực hiện việc quay đầu
            // -> Đổi chổ currentWaypointIndex và GetNextWaypointIndex() với nhau
            // Ví dụ nếu platform đang đi về start -> currentWaypointIndex = n và GetNextWaypointIndex() = n - 1
            // Khi quay đầu thì lúc này đổi chổ currentWaypointIndex = n - 1 và GetNextWaypointIndex() = n
            // Ngược lại với platform đang đi tới end

            targetPosition = waypoints[GetNextWaypointIndex()];
        }

        UpdateVelocityTowardsTarget();
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || waypoints == null || waypoints.Count < 2) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPosition, 0.1f);
        Gizmos.DrawLine(rb.position, targetPosition);
    }
}