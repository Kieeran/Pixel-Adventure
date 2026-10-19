using System;
using UnityEngine;

public class RockHeadCollision : MonoBehaviour
{
    public event Action<Vector2> OnCharacterCollided;
    [SerializeField] BoxCollider2D col;
    [SerializeField] RockHead rockHead;

    void OnValidate()
    {
        rockHead = GetComponent<RockHead>();
        if (rockHead == null)
        {
            Debug.LogError("RockHeadCollision requires a RockHead component on the same GameObject.", this);
        }

        col = GetComponentInChildren<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    void HandleCollision(Collision2D collision)
    {
        if (rockHead == null) return;

        if (collision.gameObject.CompareTag("Terrain"))
        {
            HandleTerrainCollision(collision);
        }

        if (collision.gameObject.CompareTag("Character"))
        {

        }
    }

    void HandleTerrainCollision(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint2D contact = collision.GetContact(i);
            if (rockHead.IsBlockedByTerrain(contact.normal))
            {
                // Tính khoảng cách tối thiểu giữa collider của RockHead và terrain.
                // distance < 0 nghĩa là hai collider đang chồng lấn; giá trị âm
                // biểu thị độ sâu lấn vào nhau, còn normal cung cấp hướng của phép
                // hiệu chỉnh. Nhân hai giá trị tạo offset có cả hướng lẫn độ lớn.
                ColliderDistance2D distance = col.Distance(collision.collider);
                Vector2 overlappedRetrieveOffset = Vector2.zero;
                if (distance.distance < 0f)
                {
                    overlappedRetrieveOffset = distance.normal * distance.distance;
                }

                // contact.normal dùng để xác định mặt va chạm/animation; offset
                // được tính riêng từ khoảng chồng lấn để đưa Rigidbody ra khỏi terrain.
                rockHead.OnTerrainCollision(GetTerrainCollisionSide(contact.normal), overlappedRetrieveOffset);
                return;
            }
        }
    }

    RockHead.TerrainCollisionSide GetTerrainCollisionSide(Vector2 contactNormal)
    {
        if (Mathf.Abs(contactNormal.x) > Mathf.Abs(contactNormal.y))
        {
            return contactNormal.x > 0f
                ? RockHead.TerrainCollisionSide.Left
                : RockHead.TerrainCollisionSide.Right;
        }

        return contactNormal.y > 0f
            ? RockHead.TerrainCollisionSide.Bottom
            : RockHead.TerrainCollisionSide.Top;
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Character"))
    //     {
    //         Vector2 hitPos = other.ClosestPoint(transform.parent.position);
    //         OnCharacterCollided?.Invoke(hitPos);
    //     }
    // }
}
