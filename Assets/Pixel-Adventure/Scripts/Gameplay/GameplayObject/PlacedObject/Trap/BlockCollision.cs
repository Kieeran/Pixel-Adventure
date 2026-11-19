using System;
using UnityEngine;

public class BlockCollision : MonoBehaviour
{
    public event Action<PlayerController, Vector2> OnCharacterCollided;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            if (!collision.gameObject.TryGetComponent<PlayerController>(out var controller)) return;

            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y != 0)
            {
                if (contact.normal.y > 0) OnCharacterCollided?.Invoke(controller, Vector2.down);
                else OnCharacterCollided?.Invoke(controller, Vector2.up);
            }
        }
    }
}
