using System;
using UnityEngine;

public class FallingPlatformCollision : MonoBehaviour
{
    public event Action OnCharacterCollided;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y < -0.7f)
            {
                OnCharacterCollided?.Invoke();
            }
        }
    }
}
