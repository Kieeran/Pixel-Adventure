using System;
using UnityEngine;

public class TrampolineCollision : MonoBehaviour
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
