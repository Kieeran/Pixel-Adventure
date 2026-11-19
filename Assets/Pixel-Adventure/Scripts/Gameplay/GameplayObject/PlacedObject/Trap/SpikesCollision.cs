using System;
using UnityEngine;

public class SpikesCollision : MonoBehaviour
{
    public BoxCollider2D col;
    public event Action<PlayerController, Vector2> OnCharacterCollided;

    void OnValidate()
    {
        col = GetComponentInChildren<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            PlayerController controller = null;

            if (other.attachedRigidbody.gameObject != null)
            {
                controller = other.attachedRigidbody.GetComponent<PlayerController>();
            }

            if (controller == null) return;

            // Điểm trên collider gai gần tâm player nhất, để hướng knockback không phụ thuộc vào độ dài cụm gai
            Vector2 hitPos = col.ClosestPoint(other.bounds.center);
            OnCharacterCollided?.Invoke(controller, hitPos);
        }
    }
}
