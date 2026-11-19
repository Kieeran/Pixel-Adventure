using System;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    private bool characterOnPlatform;

    public event Action<PlayerController, bool> OnCharacterCollided;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (!collision.gameObject.TryGetComponent<PlayerController>(out var controller)) return;

            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < -0.7f)
            {
                characterOnPlatform = true;
                OnCharacterCollided?.Invoke(controller, true);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (characterOnPlatform)
            {
                characterOnPlatform = false;
                OnCharacterCollided?.Invoke(null, false);
            }
        }
    }
}
