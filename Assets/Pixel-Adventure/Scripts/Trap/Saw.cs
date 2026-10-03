using System.Collections.Generic;
using UnityEngine;

public class Saw : PlacedObject
{
    [SerializeField] Animator animator;
    [SerializeField] SawCollision sawCollision;
    [SerializeField] Rigidbody2D rb;
    public Chains chains;
    // true: đi vòng 0 → 1 → 2 → 0, false: đi qua lại 0 → 1 → 2 → 1 → 0
    public bool isLoop;
    public bool renderChains = true;
    public List<SawWaypointData> waypoints;
    public float moveSpeed = 1f;

    private bool isMoving;
    private int currentWaypointIndex;
    private int direction;
    private Vector2 targetPosition;
    private Vector2 moveDirection;
    private float waitTimer;

    void OnValidate()
    {
        animator = GetComponentInChildren<Animator>();
        sawCollision = GetComponentInChildren<SawCollision>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetRenderChains(bool renderChains)
    {
        this.renderChains = renderChains;
        chains.gameObject.SetActive(renderChains);
    }

    public void SetChainsProperties(List<Vector2> chainWaypoints, Vector2 rootPos, float spacing)
    {
        chains.SetProperties(chainWaypoints, rootPos, spacing);
    }

    public override void OnSpawn()
    {
        SawData data = customData as SawData;
        isLoop = data.isLoop;
        SetRenderChains(data.renderChains);
        waypoints = new List<SawWaypointData>(data.waypoints);
        SetChainsProperties(data.chainWaypoints, data.rootPos, data.spacing);
        chains.Render();

        // Giống Platform: đợi LevelManager load level xong rồi mới tách chains ra khỏi saw
        Invoke(nameof(WaitOneSec), 0.1f);

        InitWaypointMovement();
    }

    void WaitOneSec()
    {
        chains.transform.SetParent(transform.parent, true);
    }

    public override void OnDespawn()
    {
        chains.transform.SetParent(transform);
        SetRenderChains(true);

        waypoints.Clear();
        isLoop = false;
        isMoving = false;
        currentWaypointIndex = 0;
        direction = 1;
        targetPosition = Vector2.zero;
        moveDirection = Vector2.zero;
        waitTimer = 0f;
        CancelInvoke();
    }

    void FixedUpdate()
    {
        if (waypoints.Count < 2) return;

        HandleMovement();

        rb.linearVelocity = moveDirection * moveSpeed;
    }

    void HandleMovement()
    {
        // Đứng đợi tại waypoint hiện tại theo waitTime của chính waypoint đó
        if (!isMoving)
        {
            waitTimer += Time.fixedDeltaTime;
            if (waitTimer >= waypoints[currentWaypointIndex].waitTime)
            {
                isMoving = true;
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
        }
    }

    private void InitWaypointMovement()
    {
        currentWaypointIndex = 0;
        direction = waypoints.Count > 1 ? 1 : 0;

        isMoving = false;
        waitTimer = 0f;
        moveDirection = Vector2.zero;
        if (waypoints.Count > 0) targetPosition = waypoints[0].position;
    }

    private int GetNextWaypointIndex()
    {
        // Loop: waypoint cuối nối lại về waypoint 0
        if (isLoop) return (currentWaypointIndex + direction) % waypoints.Count;

        return currentWaypointIndex + direction;
    }

    private void PrepareNextWaypoint()
    {
        // Chỉ xảy ra ở chế độ qua lại, chế độ loop thì index luôn nằm trong khoảng
        int nextIndex = GetNextWaypointIndex();
        if (nextIndex < 0 || nextIndex >= waypoints.Count)
        {
            direction *= -1;
        }

        targetPosition = waypoints[GetNextWaypointIndex()].position;
        UpdateVelocityTowardsTarget();
    }

    private void UpdateVelocityTowardsTarget()
    {
        moveDirection = (targetPosition - waypoints[currentWaypointIndex].position).normalized;
    }

#if UNITY_EDITOR
    [ContextMenu("Add waypoint")]
    void AddWaypoint()
    {
        // Waypoint mới đặt tại waypoint cuối cùng (nếu có), không thì đặt tại saw
        Vector3 spawnPos = transform.position;
        int waypointCount = 0;
        foreach (Transform tf in transform)
        {
            if (tf.gameObject.CompareTag("EditorOnly"))
            {
                spawnPos = tf.position;
                waypointCount++;
            }
        }

        GameObject o = new($"Waypoint {waypointCount}")
        {
            tag = "EditorOnly"
        };
        UnityEditor.Undo.RegisterCreatedObjectUndo(o, "Add saw waypoint");
        o.transform.SetParent(transform);
        o.transform.position = spawnPos;
        o.AddComponent<SawWaypoint>();

        UnityEditor.Selection.activeGameObject = o;
    }
#endif

    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying || waypoints == null || waypoints.Count < 2) return;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetPosition, 0.1f);
        Gizmos.DrawLine(rb.position, targetPosition);
    }
}
