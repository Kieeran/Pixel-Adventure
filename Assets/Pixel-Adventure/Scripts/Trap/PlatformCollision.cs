using System;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    private bool characterOnPlatform;

    public event Action<bool> OnCharacterCollided;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < -0.7f)
            {
                characterOnPlatform = true;
                OnCharacterCollided?.Invoke(true);
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
                OnCharacterCollided?.Invoke(false);
            }
        }
    }
}
