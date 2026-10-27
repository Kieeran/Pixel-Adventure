using System.Collections;
using UnityEngine;

public class FireUnit : MonoBehaviour
{
    private static readonly WaitForSeconds waitForSeconds = new(0.05f);
    private static readonly int IsCollidedHash = Animator.StringToHash("IsCollided");
    private static readonly int IsOnHash = Animator.StringToHash("IsOn");
    private static readonly int IsOffHash = Animator.StringToHash("IsOff");
    public Animator animator;
    public BoxCollider2D col;
    [SerializeField] float knockBackForce = 1f;
    [SerializeField] int ignitionCount;

    bool isOff = true;
    bool canDealDamage = false;

    void OnValidate()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
    }

    public void TriggerFire()
    {
        // Kiểm tra và khóa ngay lập tức để tránh Spam Trigger
        if (!isOff) return;
        isOff = false;

        StartCoroutine(FireSequenceRoutine());
    }

    IEnumerator FireSequenceRoutine()
    {
        // --- Giai đoạn 1: Lún xuống rồi trồi lên (Hit) ---
        animator.SetTrigger(IsCollidedHash);

        // Khai báo lại Bool nếu state transition yêu cầu pulse/reset
        yield return HelperFunctions.WaitCurrentAnimationEnd(animator, null);

        // Đợi một khoảng thời gian nhỏ trước khi thổi lửa
        yield return waitForSeconds;

        // --- Giai đoạn 2: Thổi lửa n lần (Tuần tự) ---
        animator.SetTrigger(IsOnHash);
        canDealDamage = true;
        int count = ignitionCount;
        do
        {
            // Chờ mỗi lần thổi lửa hoàn thành xong mới sang lần kế tiếp
            yield return HelperFunctions.WaitCurrentAnimationEnd(animator, null);

            // Reset animation cho lần thổi lửa tiếp theo (nếu có)
            animator.Play(animator.GetCurrentAnimatorStateInfo(0).shortNameHash, 0, 0.0f);

            count--;
        } while (count > 0);

        // --- Giai đoạn 3: Hoàn tất và Reset ---
        animator.SetTrigger(IsOffHash);
        isOff = true;
        canDealDamage = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            if (!canDealDamage) return;

            // Điểm trên collider gai gần tâm player nhất, để hướng knockback không phụ thuộc vào độ dài cụm gai
            Vector2 hitPos = col.ClosestPoint(other.bounds.center);
            PlayerController.Instance.playerPhysic.ReceiveDamage(hitPos, knockBackForce);
            canDealDamage = false;
        }
    }
}