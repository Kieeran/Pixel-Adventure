using System;
using UnityEngine;

public class ArrowCollision : MonoBehaviour
{
    public event Action OnCharacterCollided;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            OnCharacterCollided?.Invoke();
        }
    }
}
