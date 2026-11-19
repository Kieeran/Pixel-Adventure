using System;
using UnityEngine;

public class ArrowCollision : MonoBehaviour
{
    public event Action<PlayerController> OnCharacterCollided;

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

            OnCharacterCollided?.Invoke(controller);
        }
    }
}
