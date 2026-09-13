using System.Collections.Generic;
using UnityEngine;

public class Platform : PlacedObject
{
    private static readonly int IsOnHash = Animator.StringToHash("isOn");
    [SerializeField] Animator animator;
    public Transform brownSkin;
    public Transform greySkin;
    public List<Vector2> waypoints;
    public float moveSpeed = 1f;
    public float waitTime = 1f;

    private Rigidbody2D rb;
    private bool isMoving;
    private int currentWaypointIndex;
    private int direction;
    private Vector2 targetPosition;
    private Vector2 moveDirection;
    private float waitTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetPlatformSkin(bool isBrownPlatform)
    {
        brownSkin.gameObject.SetActive(isBrownPlatform);
        greySkin.gameObject.SetActive(!isBrownPlatform);

        animator = isBrownPlatform ? brownSkin.GetComponent<Animator>() : greySkin.GetComponent<Animator>();
    }

    public override void OnSpawn()
    {
        PlatformData data = customData as PlatformData;
        SetPlatformSkin(data.isBrownPlatform);
        waypoints = new List<Vector2>(data.waypoints);

        InitWaypointMovement();
    }

    public override void OnDespawn()
    {
        brownSkin.gameObject.SetActive(true);
        greySkin.gameObject.SetActive(true);
        waypoints.Clear();
        waitTimer = 0f;
    }

    private void InitWaypointMovement()
    {
        currentWaypointIndex = 0;
        direction = waypoints.Count > 1 ? 1 : 0;

        if (waypoints.Count == 0) return;
        if (waypoints.Count > 1)
        {
            isMoving = true;
            animator.SetBool(IsOnHash, isMoving);
            targetPosition = waypoints[GetNextWaypointIndex()];
            UpdateVelocityTowardsTarget();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (waypoints.Count < 2) return;

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
            rb.linearVelocity = Vector2.zero;
            isMoving = false;
            animator.SetBool(IsOnHash, isMoving);
            return;
        }

        rb.linearVelocity = moveDirection * moveSpeed;
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
            direction *= -1;
        }

        targetPosition = waypoints[GetNextWaypointIndex()];
        UpdateVelocityTowardsTarget();
    }

    private void UpdateVelocityTowardsTarget()
    {
        moveDirection = (targetPosition - waypoints[currentWaypointIndex]).normalized;
    }

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || waypoints == null || waypoints.Count < 2) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPosition, 0.1f);
        Gizmos.DrawLine(rb.position, targetPosition);
    }
}