using System;
using UnityEngine;

public class BoxCollision : MonoBehaviour
{
    [SerializeField] Box box;

    public Collider2D _collider;
    public float cachedColliderRadius;
    public event Action<Vector2> OnCharacterCollided;

    void OnValidate()
    {
        if (transform.TryGetComponent<Box>(out var box)) this.box = box;
        cachedColliderRadius = _collider.bounds.extents.magnitude;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            ContactPoint2D contact = collision.GetContact(0);
            if (contact.normal.y != 0)
            {
                if (contact.normal.y > 0) OnCharacterCollided?.Invoke(Vector2.down);
                else OnCharacterCollided?.Invoke(Vector2.up);
            }
        }
    }
}