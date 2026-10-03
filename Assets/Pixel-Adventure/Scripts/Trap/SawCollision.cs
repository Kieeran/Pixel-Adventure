using System;
using UnityEngine;

public class SawCollision : MonoBehaviour
{
    public event Action<Vector2> OnCharacterCollided;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Character"))
        {
            Vector2 hitPos = other.ClosestPoint(transform.parent.position);
            OnCharacterCollided?.Invoke(hitPos);
        }
    }
}
