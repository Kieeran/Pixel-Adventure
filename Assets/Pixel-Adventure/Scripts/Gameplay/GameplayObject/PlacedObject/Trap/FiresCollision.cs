using System;
using UnityEngine;

public class FiresCollision : MonoBehaviour
{
    public BoxCollider2D col;
    public event Action<Vector2> OnCharacterCollided;

    void OnValidate()
    {
        if (transform.childCount > 2) col = transform.GetChild(2).GetComponent<BoxCollider2D>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            Vector2 avg = Vector2.zero;
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactPoint2D contact = collision.GetContact(i);

                // Khi dãy Fires nhìn sang trái hoặc phải mà player va chạm theo chiều dọc thì không tính va chạm (kích hoạt bẫy)
                bool condition1 = IsFacing(transform, Vector2.left) || IsFacing(transform, Vector2.right);
                condition1 = condition1 && (contact.normal.y > 0.7f || contact.normal.y < -0.7f);

                // Khi dãy Fires nhìn lên trên hoặc xuống dưới mà player va chạm theo chiều ngang thì không tính va chạm (kích hoạt bẫy)
                bool condition2 = IsFacing(transform, Vector2.up) || IsFacing(transform, Vector2.down);
                condition2 = condition2 && (contact.normal.x != 0);

                if (condition1 || condition2) return;

                avg += contact.point;
            }

            OnCharacterCollided?.Invoke(avg / collision.contactCount);
        }
    }

    bool IsFacing(Transform tf, Vector2 dir)
    {
        return Vector2.Dot(tf.up, dir) > 0.99f;
    }
}
