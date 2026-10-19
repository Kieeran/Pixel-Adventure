using System;
using System.Collections.Generic;
using UnityEngine;

public class RockHeadCollision : MonoBehaviour
{
    public event Action<bool> OnCharacterRidingStateChanged;
    public event Action<Vector2> OnCharacterCrushed;
    [SerializeField] BoxCollider2D col;
    [SerializeField] RockHead rockHead;

    private bool characterOnRockHead;
    readonly List<ContactPoint2D> bContacts = new();

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
        if (collision.gameObject.CompareTag("Terrain"))
        {
            HandleTerrainCollision(collision);
        }

        if (collision.gameObject.CompareTag("Character"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < -0.7f)
            {
                characterOnRockHead = true;
                OnCharacterRidingStateChanged?.Invoke(true);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Terrain"))
        {
            HandleTerrainCollision(collision);
        }

        if (collision.gameObject.CompareTag("Character"))
        {
            // A - RockHead; B - Player; C - Terrain
            Collider2D b = collision.collider;

            // Hướng B bị A đẩy (normal chỉ hướng A bị đẩy ra, nên đảo dấu)
            Vector2 pushOnB = -collision.GetContact(0).normal;

            bContacts.Clear();
            b.GetContacts(bContacts);

            foreach (var cp in bContacts)
            {
                Collider2D c = cp.collider == b ? cp.otherCollider : cp.collider;
                if (c.attachedRigidbody == collision.otherRigidbody) continue;   // bỏ A
                if (c.attachedRigidbody != null && c.attachedRigidbody.bodyType != RigidbodyType2D.Static) continue;    // chỉ quan tâm vật tĩnh

                Vector2 pushOnBFromC = cp.otherCollider == b ? cp.normal : -cp.normal;

                if (Vector2.Dot(pushOnB, pushOnBFromC) < -0.7f)
                {
                    // B bị kẹp giữa A và C
                    OnCharacterCrushed?.Invoke(pushOnB);
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (characterOnRockHead)
            {
                characterOnRockHead = false;
                OnCharacterRidingStateChanged?.Invoke(false);
            }
        }
    }

    void HandleTerrainCollision(Collision2D collision)
    {
        if (rockHead == null) return;

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
}
