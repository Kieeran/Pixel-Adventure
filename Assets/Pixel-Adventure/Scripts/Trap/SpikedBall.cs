using System.Collections.Generic;
using UnityEngine;

// NOTE: Ôn luyện thêm về góc có hướng, vận tốc/gia tốc góc và động học khi hãm vật.
public class SpikedBall : PlacedObject
{
    [SerializeField] SpikedBallCollision spikedBallCollision;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float knockBackForce = 1f;

    public Transform ball;
    public Chains chains;
    [Tooltip("Rotate continuously through 360 degrees instead of swinging between the angle limits.")]
    public bool rotateContinuously;
    [Tooltip("Rotation direction used by continuous rotation. Enabled means clockwise.")]
    public bool rotateClockwise;
    [Tooltip("Starting angle relative to the root's rotation when spawned, used by continuous rotation.")]
    public float continuousStartAngle;
    [Tooltip("Angular speed in degrees per second.")]
    public float rotationSpeed = 90f;
    [Tooltip("First swing limit relative to the root's rotation when spawned. The ball starts here.")]
    public float swingStartAngle = -45f;
    [Tooltip("Second swing limit relative to the root's rotation when spawned.")]
    public float swingEndAngle = 45f;
    [Tooltip("Acceleration and deceleration while swinging, in degrees per second squared.")]
    public float swingAcceleration = 180f;

    float rotationBase;
    int swingDirection;
    bool rotationInitialized;
    float swingCurrentSpeed;

    void OnValidate()
    {
        spikedBallCollision = GetComponent<SpikedBallCollision>();
        rb = GetComponent<Rigidbody2D>();
        if (chains == null) chains = GetComponentInChildren<Chains>(true);
    }

    public void SetChainsProperties(List<Vector2> chainWaypoints, Vector2 rootPos, float spacing, Sprite skin)
    {
        chains.SetProperties(chainWaypoints, rootPos, spacing, skin);
    }

    public void SetBallOffset(Vector2 offset)
    {
        Vector3 localPosition = ball.localPosition;
        ball.localPosition = new Vector3(offset.x, offset.y, localPosition.z);
    }

    public override void OnSpawn()
    {
        spikedBallCollision.OnCharacterCollided += OnCharacterCollided;
        rb.angularVelocity = 0f;
        rotationInitialized = false;

        if (customData is SpikedBallData data) data.ApplyTo(this);

        // PoolManager đặt vị trí và góc của object sau OnSpawn; chờ một nhịp để đọc được transform đã đặt.
        Invoke(nameof(Wait), 0.1f);
    }

    void Wait()
    {
        InitializeRotation();
    }

    public override void OnDespawn()
    {
        spikedBallCollision.OnCharacterCollided -= OnCharacterCollided;
        rb.angularVelocity = 0f;
        rotationInitialized = false;
        swingDirection = 0;
        swingCurrentSpeed = 0f;
    }

    void FixedUpdate()
    {
        // Không cập nhật physics cho tới khi góc gốc và trạng thái bắt đầu đã được thiết lập.
        if (!rotationInitialized) return;

        // Rigidbody2D.angularVelocity dùng độ/giây. Dấu của vận tốc biểu thị chiều quay,
        // vì vậy rotationSpeed lấy trị tuyệt đối còn hướng được xử lý riêng.
        float speed = Mathf.Abs(rotationSpeed);
        if (speed <= 0f)
        {
            rb.angularVelocity = 0f;
            return;
        }

        if (rotateContinuously)
        {
            // 2D: vận tốc góc dương quay ngược chiều kim đồng hồ, âm quay cùng chiều.
            // Rigidbody tiếp tục tích phân góc mỗi bước physics, nên không cần giới hạn góc ở 360.
            rb.angularVelocity = rotateClockwise ? -speed : speed;
            return;
        }

        if (swingDirection == 0)
        {
            rb.angularVelocity = 0f;
            return;
        }

        // Đổi góc hiện tại sang độ lệch có dấu so với root ban đầu; từ đó xét chuyển động
        // trong hệ quy chiếu cố định, không phụ thuộc cách Unity biểu diễn góc tuyệt đối.
        float currentAngle = Mathf.DeltaAngle(rotationBase, rb.rotation);
        float targetAngle = swingDirection > 0 ? swingEndAngle : swingStartAngle;
        float distanceToTarget = GetDistanceInDirection(currentAngle, targetAngle, swingDirection);
        // Gia tốc âm không có ý nghĩa trong phép tính hãm này, nên xem như bằng 0.
        float acceleration = Mathf.Max(0f, swingAcceleration);

        // v = v0 + a*dt: tăng tốc dần nhưng không vượt quá tốc độ cấu hình.
        swingCurrentSpeed = Mathf.Min(speed, swingCurrentSpeed + acceleration * Time.fixedDeltaTime);
        // Từ v² = u² + 2as, với vận tốc cuối u = 0, suy ra
        // v_max = sqrt(2*a*d). Đây là tốc độ lớn nhất còn đủ quãng đường để dừng ở biên.
        float maxSpeedBeforeTarget = Mathf.Sqrt(2f * acceleration * distanceToTarget);
        swingCurrentSpeed = Mathf.Min(swingCurrentSpeed, maxSpeedBeforeTarget);

        // Nếu trong bước physics kế tiếp sẽ chạm/vượt biên, đặt chính xác tại biên,
        // triệt tiêu tốc độ rồi đảo hướng để tránh trôi quá giới hạn do bước thời gian rời rạc.
        if (distanceToTarget <= swingCurrentSpeed * Time.fixedDeltaTime)
        {
            rb.rotation = rotationBase + targetAngle;
            swingCurrentSpeed = 0f;
            swingDirection *= -1;
            rb.angularVelocity = 0f;
            return;
        }

        // Dấu xác định hướng; độ lớn là tốc độ góc hiện tại tính bằng độ/giây.
        rb.angularVelocity = swingDirection * swingCurrentSpeed;
    }

    void OnCharacterCollided(Vector2 hitPos)
    {
        PlayerController.Instance.playerPhysic.ReceiveDamage(hitPos, knockBackForce);
    }

    void InitializeRotation()
    {
        // Góc đặt của root là mốc 0 tương đối cho các góc start/end, không phải góc world cố định.
        rotationBase = transform.eulerAngles.z;
        rotationInitialized = true;

        if (rotateContinuously)
        {
            // Đặt góc ban đầu một lần; sau đó FixedUpdate chỉ duy trì vận tốc góc liên tục.
            rb.rotation = rotationBase + continuousStartAngle;
            return;
        }

        // Chế độ lắc luôn bắt đầu tại biên A.
        rb.rotation = rotationBase + swingStartAngle;
        swingCurrentSpeed = 0f;
        // DeltaAngle lấy đường ngắn nhất giữa hai biên và cho biết cần đi theo chiều nào.
        float angleFromStartToEnd = Mathf.DeltaAngle(swingStartAngle, swingEndAngle);
        swingDirection = Mathf.Approximately(angleFromStartToEnd, 0f)
            ? 0
            : angleFromStartToEnd > 0f ? 1 : -1;
    }

    static float GetDistanceInDirection(float fromAngle, float toAngle, int direction)
    {
        // Góc xoay có tính chu kỳ 360 độ. Repeat biến hiệu góc thành khoảng cách dương
        // theo đúng chiều đang đi, kể cả khi đi qua mốc 0/360.
        return direction > 0
            ? Mathf.Repeat(toAngle - fromAngle, 360f)
            : Mathf.Repeat(fromAngle - toAngle, 360f);
    }
}
