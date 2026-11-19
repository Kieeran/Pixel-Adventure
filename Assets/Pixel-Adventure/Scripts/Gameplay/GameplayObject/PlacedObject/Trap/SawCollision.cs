using System;
using UnityEngine;

public class SawCollision : MonoBehaviour
{
    public event Action<PlayerController, Vector2> OnCharacterCollided;

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

            Vector2 hitPos = other.ClosestPoint(transform.parent.position);
            OnCharacterCollided?.Invoke(controller, hitPos);
        }
    }
}
